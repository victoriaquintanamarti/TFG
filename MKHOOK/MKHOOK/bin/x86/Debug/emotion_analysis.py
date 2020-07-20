# encoding: utf-8
#from translate import translator
import pickle
import os
import time
import matplotlib.pyplot as plt
import pandas as pd
from math import pi
import shutil
from time import localtime, strftime


def draw_graphic(list1,ax):
    # Set data
    plt.cla()
    size_list = (len(list1)/4)-1
    emotions = [0.0,0.0,0.0,0.0]
    print(emotions[0])
    aux = False
    aux2 = False
    for x in range(len(list1)):
        if (x % 4 == 0) or (x == 0):
            emotions[0] = emotions[0] + list1[x]
            aux = True
        elif (aux and not aux2):
            emotions[1] = emotions[1] + list1[x]  
            aux = False
        elif not aux and not aux2:
            emotions[2] = emotions[2] + list1[x]
            aux2 = True
        elif not aux and aux2:
            emotions[3] = emotions[3] + list1[x]
            aux2 = False 
    print(emotions)
    print(size_list)
    for n in range(len(emotions)):
        emotions[n] = (emotions[n]/size_list)
    print(emotions)
    df = pd.DataFrame({
    'group': ['emotions'],
    'anger': [emotions[0]],
    'fear': [emotions[1]],
    'joy': [emotions[2]],
    'sadness': [emotions[3]]
    })
    
    # number of variable
    categories=list(df)[1:]
    N = len(categories)
    
    # We are going to plot the first line of the data frame.
    # But we need to repeat the first value to close the circular graph:
    values=df.loc[0].drop('group').values.flatten().tolist()
    values += values[:1]
    values
    
    # What will be the angle of each axis in the plot? (we divide the plot / number of variable)
    angles = [n / float(N) * 2 * pi for n in range(N)]
    angles += angles[:1]
    
    # Draw one axe per variable + add labels labels yet
    plt.xticks(angles[:-1], categories, color='grey', size=8)
    
    # Draw ylabels
    ax.set_rlabel_position(0)
    plt.yticks([ 0, 20, 40, 60, 80], ["0", "20", "40", "60", "80"], color="grey", size=7)
    plt.ylim(0,80)
    
    # Plot data
    ax.plot(angles, values, linewidth=1, linestyle='solid')
    
    # Fill area
    ax.fill(angles, values, 'b', alpha=0.1)
    d4 = strftime("%Y-%m-%d %H-%M-%S", localtime())
    plt.savefig('pictures/emotions chart'+d4+'.png')
    return list1

path = os.path.abspath("words2.txt")

fileToRead = open(path)
loaded_model = pickle.load(open('finalized_model.sav', 'rb'))
vectorizer = pickle.load(open('vectorizer.pickle', 'rb'))
list1 = [0,0,0,0]
ax = plt.subplot(111, polar=True)

while 1:
    where = fileToRead.tell()
    line = fileToRead.readline()
    phraseOrigin = line
    phrase = phraseOrigin.lower()
    #phrase = translator('en', 'es', phraseOrigin)
    
    if not line:
        time.sleep(1)
        fileToRead.seek(where)
    else: 
        print(phrase)
        proba = loaded_model.predict_proba(vectorizer.transform([phrase]))
        print(proba)
        f = open("proba.txt", 'a', encoding='utf-8')
        b = '\n'.join(' '.join('%0.2f' %x for x in y) for y in proba)
        for y in proba:
            for x in y:
                num = float('%0.2f' %x)
                list1.append(num*100)
        if phrase != '\n':
            f.write(b)
            f.write('\n')
        f.close()
        draw_graphic(list1,ax)