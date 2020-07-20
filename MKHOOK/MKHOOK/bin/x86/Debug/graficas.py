import logging
import numpy as np
from optparse import OptionParser
import sys
from time import time
import pandas as pd
import matplotlib.pyplot as plt
import re

from sklearn.datasets import fetch_20newsgroups
from sklearn.feature_extraction.text import TfidfVectorizer
from sklearn.feature_extraction.text import HashingVectorizer
from sklearn.feature_selection import SelectFromModel
from sklearn.feature_selection import SelectKBest, chi2
from sklearn.linear_model import RidgeClassifier
from sklearn.pipeline import Pipeline
from sklearn.preprocessing import LabelEncoder
from sklearn.svm import LinearSVC
from sklearn.linear_model import SGDClassifier
from sklearn.svm import SVC
from sklearn.linear_model import Perceptron
from sklearn.model_selection import train_test_split
from scipy.stats import itemfreq
from sklearn.feature_extraction.text import CountVectorizer,TfidfTransformer,HashingVectorizer
from sklearn.linear_model import PassiveAggressiveClassifier
from sklearn.naive_bayes import BernoulliNB, ComplementNB, MultinomialNB
from sklearn.neighbors import KNeighborsClassifier
from sklearn.neighbors import NearestCentroid
from sklearn.ensemble import RandomForestClassifier
from sklearn.utils.extmath import density
from sklearn import metrics
import nltk
from nltk.corpus import stopwords
from nltk import word_tokenize          
from nltk.stem import PorterStemmer 
from nltk.tokenize import word_tokenize 
from textblob import TextBlob
from nltk import word_tokenize 
from nltk.stem import WordNetLemmatizer 
from nltk.stem.wordnet import WordNetLemmatizer
from nltk.corpus import wordnet
import pickle
from sklearn.linear_model import LogisticRegression
from sklearn.calibration import CalibratedClassifierCV

lemmatizer = nltk.stem.WordNetLemmatizer()
wordnet_lemmatizer = WordNetLemmatizer()

pd.options.mode.chained_assignment = None

def print_acc(model):
    predicted = model.predict(X_test.content)
    accuracy = np.mean(predicted == y_test) * 100
    print(accuracy)

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

def lemmatize_sentence(sentence):
    #tokenize the sentence and find the POS tag for each token
    nltk_tagged = nltk.pos_tag(nltk.word_tokenize(sentence))
    #tuple of (token, wordnet_tag)
    wordnet_tagged = map(lambda x: (x[0], nltk_tag_to_wordnet_tag(x[1])), nltk_tagged)
    lemmatized_sentence = []
    for word, tag in wordnet_tagged:
        if tag is None:
            #if there is no available tag, append the token as is
            lemmatized_sentence.append(word)
        else:
            #else use the tag to lemmatize the token
            lemmatized_sentence.append(lemmatizer.lemmatize(word, tag))
    return " ".join(lemmatized_sentence)

data1 = pd.read_csv('text_emotion.csv',encoding = "ISO-8859-1")
data1=data1[['tweet_id','sentiment','content']].copy()
data1.sentiment = np.where((data1.sentiment == 'neutral') |(data1.sentiment == 'empty')|(data1.sentiment == 'boredom'),'neutral',data1.sentiment)
data1.sentiment = np.where((data1.sentiment == 'fun') |(data1.sentiment == 'enthusiasm'),'fun',data1.sentiment)
data1=data1[data1.sentiment !='neutral']

data2=pd.read_csv('tweets_clean.txt',sep='	',header=None)
data2.columns=['tweet_id','content','sentiment']
data2.sentiment = data2.sentiment.str.replace(':: ','')

data3=pd.read_csv('isear.csv',sep=';')
data3=data3[['sentiment','content']].copy()

data4=pd.read_csv('Emotion Phrases.csv',sep=',')
data4=data4[['sentiment','content']].copy()

data5=pd.read_csv('NRC-Emotion-Intensity-Lexicon-v1.txt',sep='	')
data5=data5[['content','sentiment']].copy()
data5=data5[data5.sentiment !='anticipation']

data = pd.concat([data3,data4])

print(data.sentiment.value_counts())
stop_words = set(stopwords.words('english'))
data.sentiment = np.where((data.sentiment == 'disgust') |(data.sentiment == 'hate'),'hate',data.sentiment)
data=data[data.sentiment.isin(['anger','fear','joy','sadness'])]
data['content']=data['content'].str.replace('[^A-Za-z0-9\s]+', '')
data['content']=data['content'].str.replace('http\S+|www.\S+', '', case=False)
data['content'] = [w for w in data['content'] if not w in stop_words] 
# Lemmatizing
data['content'] = data['content'].apply(lambda x: lemmatize_sentence(x))
print (data['content'])

target=data.sentiment
data = data.drop(['sentiment'],axis=1)
le=LabelEncoder()
target=le.fit_transform(target)
X_train, X_test, y_train, y_test = train_test_split(data,target,stratify=target,test_size=0.1, random_state=42)

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

results.append(benchmark(BernoulliNB()))

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
clf_names.append("BernoulliNB")
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
plt.barh(indices, scores2, .2, label="score", color='navy')
plt.barh(indices + .3, training_time_new, .2, label="training time", color='c')
plt.barh(indices + .6, test_time_new, .2, label="test time", color='darkorange')
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

plt.show()
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

