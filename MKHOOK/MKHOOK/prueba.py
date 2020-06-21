from googletrans import Translator
import pickle
import os

path = os.path.abspath("words.txt")

translator = Translator()
fileToRead = open(path)
for line in fileToRead:
    phraseOrigin = line
    translation = translator.translate(phraseOrigin)
    phrase = translation.text.lower()
    print(phrase)
    loaded_model = pickle.load(open('finalized_model.sav', 'rb'))
    vectorizer = pickle.load(open('vectorizer.pickle', 'rb'))
    print(loaded_model.predict(vectorizer.transform([phrase])))
    print(loaded_model.predict_proba(vectorizer.transform([phrase])))
fileToRead.close()