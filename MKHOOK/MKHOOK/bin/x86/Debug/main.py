import pandas as pd
import numpy as np
import re

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
lemmatizer = nltk.stem.WordNetLemmatizer()
wordnet_lemmatizer = WordNetLemmatizer()
from sklearn.feature_selection import SelectFromModel
from sklearn.svm import LinearSVC
from sklearn.svm import SVC
from sklearn.naive_bayes import BernoulliNB, ComplementNB, MultinomialNB

from scipy.stats import itemfreq
from sklearn.model_selection import train_test_split
from sklearn.linear_model import SGDClassifier
from sklearn.naive_bayes import MultinomialNB
from sklearn.pipeline import Pipeline
from sklearn.preprocessing import LabelEncoder
from sklearn.feature_extraction.text import CountVectorizer,TfidfTransformer,HashingVectorizer
from sklearn.metrics import confusion_matrix
from sklearn.naive_bayes import BernoulliNB
from googletrans import Translator
import pickle
from sklearn.metrics import confusion_matrix


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

# Machine Learning
# Training Naive Bayes (NB) classifier on training data.
clf = MultinomialNB().fit(X_train_counts,y_train)
predicted = clf.predict(X_test_counts)
nb_clf_accuracy = np.mean(predicted == y_test) * 100
conf_mat = confusion_matrix(y_test,predicted)
print(conf_mat)
print(nb_clf_accuracy)
#print(predicted)
#print(y_test)
'''
clf2 = ComplementNB().fit(X_train_counts,y_train)
predicted2 = clf2.predict(X_test_counts)
nb_clf_accuracy2 = np.mean(predicted2 == y_test) * 100

print(nb_clf_accuracy2)

clf3 = BernoulliNB().fit(X_train_counts,y_train)
predicted3 = clf3.predict(X_test_counts)
nb_clf_accuracy3 = np.mean(predicted3 == y_test) * 100

print(nb_clf_accuracy3)
'''
filename = 'finalized_model.sav'
pickle.dump(clf, open(filename, 'wb'))
pickle.dump(count_vect, open("vectorizer.pickle", "wb"))

phraseOrigin = "TE ODIO"
translator = Translator()
translation = translator.translate(phraseOrigin)
phrase = translation.text

print(phrase)
print(clf.predict(count_vect.transform([phrase])))
print(clf.predict_proba(count_vect.transform([phrase])))
'''
print(clf2.predict(count_vect.transform([phrase])))
print(clf2.predict_proba(count_vect.transform([phrase])))
print(clf3.predict(count_vect.transform([phrase])))
print(clf3.predict_proba(count_vect.transform([phrase])))
'''