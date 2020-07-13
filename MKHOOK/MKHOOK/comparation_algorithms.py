# Librerías utilizadas

import numpy as np
import sys
from time import time
import pandas as pd
import matplotlib.pyplot as plt
import re

from sklearn.pipeline import Pipeline
from sklearn.preprocessing import LabelEncoder
from sklearn.feature_selection import SelectFromModel
from sklearn.feature_selection import SelectKBest, chi2
from sklearn.svm import LinearSVC
from sklearn.svm import SVC
from sklearn.model_selection import train_test_split
from scipy.stats import itemfreq
from sklearn.feature_extraction.text import CountVectorizer,TfidfTransformer,HashingVectorizer
from sklearn.naive_bayes import BernoulliNB, ComplementNB, MultinomialNB
from sklearn.neighbors import KNeighborsClassifier
from sklearn.neighbors import NearestCentroid
from sklearn import metrics
import nltk
from nltk.corpus import stopwords
from nltk import word_tokenize          
from nltk.stem import PorterStemmer 
from nltk.tokenize import word_tokenize 
from textblob import TextBlob
from nltk.stem import WordNetLemmatizer 
from nltk.stem.wordnet import WordNetLemmatizer
from nltk.corpus import wordnet
import pickle
from sklearn.linear_model import LogisticRegression
from sklearn.calibration import CalibratedClassifierCV

# Se inicializa el lemmatizer, el corpus del lemmatizer y el stemmer
lemmatizer = nltk.stem.WordNetLemmatizer()
wordnet_lemmatizer = WordNetLemmatizer()
ps = PorterStemmer()

pd.options.mode.chained_assignment = None

# Función que te indica la precisión del modelo obtenido

def print_acc(model):
    predicted = model.predict(X_test.content)
    accuracy = np.mean(predicted == y_test) * 100
    print(accuracy)

# Función para obtener el tipo de palabra para realizar la lematización.

def nltk_tag_to_wordnet_tag(nltk_tag):
    if nltk_tag.startswith('J'):
        return wordnet.ADJ
    elif nltk_tag.startswith('V'):
        return wordnet.VERB
    elif nltk_tag.startswith('N'):
        return wordnet.NOUN
    elif nltk_tag.startswith('R'):
        return wordnet.ADV
    else:
        return None

# Función para realizar la lematización

def lemmatize_sentence(sentence):
    # tokeniza la frase y encuentra el tag para cada token
    nltk_tagged = nltk.pos_tag(nltk.word_tokenize(sentence))
    # tupla de (token, wordnet_tag)
    wordnet_tagged = map(lambda x: (x[0], nltk_tag_to_wordnet_tag(x[1])), nltk_tagged)
    lemmatized_sentence = []
    for word, tag in wordnet_tagged:
        if tag is None:
            # si no existe una etiqueta (tag) disponible, se pone el token como esté
            lemmatized_sentence.append(word)
        else:
            # en cambio si existe el tag se usa para lematizar el token
            lemmatized_sentence.append(lemmatizer.lemmatize(word, tag))
    return " ".join(lemmatized_sentence)

# Función para realizar el stemming

def stemming_sentence (sentence):
    token_sentence = nltk.word_tokenize(sentence)
    stemmed_sentence = []
    for word in token_sentence:
        word = ps.stem(word)
        stemmed_sentence.append(word)

    return " ".join(stemmed_sentence)
  

# Se realiza la carga de los datasets

data1=pd.read_csv('isear.csv',sep=';')
data1=data1[['sentiment','content']].copy()

data2=pd.read_csv('Emotion Phrases.csv',sep=',')
data2=data2[['sentiment','content']].copy()

data = pd.concat([data1,data2])

print(data.sentiment.value_counts())
stop_words = set(stopwords.words('english'))
data.sentiment = np.where((data.sentiment == 'disgust') |(data.sentiment == 'hate'),'hate',data.sentiment)
data=data[data.sentiment.isin(['anger','fear','joy','sadness'])]
data['content']=data['content'].str.replace('[^A-Za-z0-9\s]+', '')
data['content']=data['content'].str.lower()
data['content']=data['content'].str.replace('http\S+|www.\S+', '', case=False)
data['content'] = [w for w in data['content'] if not w in stop_words] 
# Stemming
data['content'] = data['content'].apply(lambda x: stemming_sentence(x))
# Lemmatizing
data['content'] = data['content'].apply(lambda x: lemmatize_sentence(x))
print (data['content'])

target=data.sentiment
data = data.drop(['sentiment'],axis=1)
le=LabelEncoder()
target=le.fit_transform(target)
X_train, X_test, y_train, y_test = train_test_split(data,target,stratify=target,test_size=0.2, random_state=42)

itemfreq(y_train)
itemfreq(y_test)

# Extracting features from text files (Convert a collection of text documents to a matrix of token counts)
count_vect = CountVectorizer()
X_train_counts = count_vect.fit_transform(X_train.content)
X_test_counts = count_vect.transform(X_test.content)

clf_names = []
scores = []
training_time = []
test_times = []
model = []
def benchmark(clf):
    print('_' * 80)
    print("Training: ")
    print(clf)
    t0 = time()
    model.append(clf.fit(X_train_counts,y_train))
    train_time = time() - t0
    training_time.append(train_time)
    print("train time: %0.3fs" % train_time)

    t0 = time()
    pred = clf.predict(X_test_counts)
    test_time = time() - t0
    test_times.append(test_time)
    print("test time:  %0.3fs" % test_time)

    score = metrics.accuracy_score(y_test, pred)
    scores.append(score)
    print("accuracy:   %0.3f" % score)


results = []

results.append(benchmark(KNeighborsClassifier(n_neighbors=10)))
results.append(benchmark(LogisticRegression(random_state=0)))

# Train NearestCentroid without threshold
print('=' * 80)
print("NearestCentroid (aka Rocchio classifier)")
results.append(benchmark(NearestCentroid()))

# Train sparse Naive Bayes classifiers
print('=' * 80)
print("Naive Bayes")
results.append(benchmark(MultinomialNB()))

results.append(benchmark(ComplementNB()))


print('=' * 80)
print("LinearSVC with L1-based feature selection")
# The smaller C, the stronger the regularization.
# The more regularization, the more sparsity.
results.append(benchmark(CalibratedClassifierCV(Pipeline([
  ('feature_selection', SelectFromModel(LinearSVC(penalty="l1", dual=False,
                                                  tol=1e-3,max_iter=1200000))),
  ('classification', LinearSVC(penalty="l2",max_iter=1200000))]))))

results.append(benchmark(SVC(probability=True)))


##############################################################################
# Add plots
# ------------------------------------
# The bar plot indicates the accuracy, training time (normalized) and test time
# (normalized) of each classifier.

indices = np.arange(len(results))

clf_names.append("kNN")
clf_names.append("LogisticRegression")
clf_names.append("NearestCentroid")
clf_names.append("MultinomialNB")
clf_names.append("ComplementNB")
clf_names.append("LinearSVC")
clf_names.append("SVC")

clf_names2 = []
scores2 = []
training_time2 = []
test_times2 = []
models2 = []

for s,c,t,test,models in sorted(zip(scores,clf_names,training_time,test_times,model)):
    scores2.append(s)
    clf_names2.append(c)
    training_time2.append(t)
    test_times2.append(test)
    models2.append(models)
    

training_time_new = np.array(training_time2) / np.max(training_time2)
test_time_new = np.array(test_times2) / np.max(test_times2)

plt.figure(figsize=(15, 9))
plt.title("Score")
plt.barh(indices, scores2, .2, label="accuracy", color='navy')
plt.yticks(())
plt.legend(loc='lower right')
plt.subplots_adjust(left=.25)
plt.subplots_adjust(top=.95)
plt.subplots_adjust(bottom=.05)


for index, value in enumerate(scores2):
    values = str(round(float(value)*100,2))
    plt.text(value, index, values + '%')


for i, c in zip(indices, clf_names2):
    plt.text(-.22, i, c)

plt.savefig('tabla.png')

i=len(models2)-1

result = None
while result is None:
    try:
        models2[i].predict_proba(count_vect.transform(["I am afraid of the dark"]))
        result = i
    except:
        i=i-1
print(clf_names2[i])
print(scores2[i])
filename = 'finalized_model.sav'
pickle.dump(models2[i], open(filename, 'wb'))
pickle.dump(count_vect, open("vectorizer.pickle", "wb"))

