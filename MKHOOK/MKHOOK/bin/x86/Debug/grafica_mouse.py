import json
import os
import matplotlib.pyplot as plt
import pandas as pd
from math import pi
import matplotlib.transforms as mtrans
from time import localtime, strftime

pressedKeys = 0
scapekey = 0
twopressedkeys = 0
mouseclicks = 0
euclideandistance = 0
mousewheel = 0
size = 0
meanPressedKeys = 0
meanScapekey = 0
meanTwopressedkeys = 0
meanMouseclicks = 0
meanEuclideandistance = 0
meanMousewheel = 0

pressedKeysAlarm = 0
scapekeyAlarm = 0
twopressedkeysAlarm = 0
mouseclicksAlarm = 0
euclideandistanceAlarm = 0
mousewheelAlarm = 0

def json_read(listMean):
    dir='infoActivity/' 
    files = sorted([ f for f in os.listdir(dir)])
    print (files[-1])

    for fileToDelete in files:
        if (fileToDelete != files[-1]):
            os.remove('infoActivity/' + fileToDelete)

    with open('infoActivity/'+ files[-1]) as json_file:
        data = json.load(json_file)
        pressedKeys = 0
        scapekey = 0
        twopressedkeys = 0
        mouseclicks = 0
        euclideandistance = 0
        mousewheel = 0
        size = 0
        for p in data['Activity']:
            pressedKeys = pressedKeys + p['Keyboard']['PressedKeys']
            scapekey = scapekey + p['Keyboard']['ScapeKey']
            twopressedkeys = twopressedkeys + p['Keyboard']['TwoPressedKeys']
            mouseclicks = mouseclicks + p['Mouse']['MouseClicks']
            euclideandistance = euclideandistance + p['Mouse']['EuclideanDistance']
            mousewheel = mousewheel + p['Mouse']['MouseWheel']
            size = size + 1
    listMean[0] = pressedKeys/size
    listMean[1] = scapekey/size
    listMean[2] = twopressedkeys/size
    listMean[3] = mouseclicks/size
    listMean[4] = euclideandistance/size
    listMean[5] = mousewheel/size

    return listMean
    
def json_alarm(listAlarm):
    with open('alarms.json') as json_file:
        data = json.load(json_file)
        listAlarm[0] = data['keyPressedAlarm']
        listAlarm[1] = data['scapeKeyAlarm']
        listAlarm[2] = data['twoPressedKeysAlarm']
        listAlarm[3] = data['mouseClicksAlarm']
        listAlarm[4] = data['euclideanDistanceAlarm']
        listAlarm[5] = data['mouseWheelAlarm']
    return listAlarm

def draw_grafic_keyboard(listFinalAlarm,ax):
    df = pd.DataFrame({
    'group': ['pressedKeys','scapekey','twopressedkeys',''],
    'Pressed Keys': [listFinalAlarm[0], 0, 0, 0],
    'Scape key': [listFinalAlarm[1], 0, 0, 0],
    'Two Pressed keys': [listFinalAlarm[2], 0, 0,0]
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
    trans1 = mtrans.Affine2D().translate(30, 0)
    trans2 = mtrans.Affine2D().translate(0, 5)
    trans3 = mtrans.Affine2D().translate(0, -10)
    aux = 0
    for t in ax.get_xticklabels():
        if(aux == 0):
            t.set_transform(t.get_transform()+trans1)
        if(aux == 1):
            t.set_transform(t.get_transform()+trans2)
        if(aux == 2):
            t.set_transform(t.get_transform()+trans3)
        aux = aux+1

    
    # Draw ylabels
    ax.set_rlabel_position(0)
    plt.yticks([0,1], ["0", "1"], color="grey", size=7)
    plt.ylim(0,1)
    
    # Plot data
    ax.plot(angles, values, linewidth=1, linestyle='solid',color='r')
    
    # Fill area
    ax.fill(angles, values, 'r', alpha=0.1)
    d4 = strftime("%Y-%m-%d %H-%M-%S", localtime())
    plt.savefig('keyboard/keyboard'+d4+'.png')
    return ax

def draw_grafic_mouse(listFinalAlarm,ax):
    df = pd.DataFrame({
    'group': ['mouseClicksAlarm','euclideanDistanceAlarm','mouseWheelAlarm',''],
    'Mouse Clicks': [listFinalAlarm[3], 0, 0, 0],
    'Euclidean Distance': [listFinalAlarm[4], 0, 0, 0],
    'Mouse Wheel': [listFinalAlarm[5], 0, 0, 0]
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
    trans1 = mtrans.Affine2D().translate(30, 0)
    trans2 = mtrans.Affine2D().translate(0, 10)
    trans3 = mtrans.Affine2D().translate(0, -10)
    aux = 0
    for t in ax.get_xticklabels():
        if(aux == 0):
            t.set_transform(t.get_transform()+trans1)
        if(aux == 1):
            t.set_transform(t.get_transform()+trans2)
        if(aux == 2):
            t.set_transform(t.get_transform()+trans3)
        aux = aux+1
    
    # Draw ylabels
    ax.set_rlabel_position(0)
    plt.yticks([ 0, 1], ["0", "1"], color="grey", size=7)
    plt.ylim(0,1)
    
    # Plot data
    ax.plot(angles, values, linewidth=1, linestyle='solid',color='r')
    
    # Fill area
    ax.fill(angles, values, 'r', alpha=0.1)
    d4 = strftime("%Y-%m-%d %H-%M-%S", localtime())
    plt.savefig('mouse/mouse'+d4+'.png')
    return ax

def set_alarms(listMean,listAlarm,listFinalAlarm):
    if(listMean[0]>=listAlarm[0]):
        listFinalAlarm[0] = 1
    else:
        listFinalAlarm[0] = 0 

    if(listMean[1]>=listAlarm[1]):
        listFinalAlarm[1] = 1
    else:
        listFinalAlarm[1] = 0 

    if(listMean[2]>=listAlarm[2]):
        listFinalAlarm[2] = 1
    else:
        listFinalAlarm[2] = 0 

    if(listMean[3]>=listAlarm[3]):
        listFinalAlarm[3] = 1
    else:
        listFinalAlarm[3] = 0 

    if(listMean[4]>=listAlarm[4]):
        listFinalAlarm[4] = 1
    else:
        listFinalAlarm[4] = 0 

    if(listMean[5]>=listAlarm[5]):
        listFinalAlarm[5] = 1
    else:
        listFinalAlarm[5] = 0 
    return listFinalAlarm

def main_alarms(listMean,listAlarm,ax,listFinalAlarm):
    pressedKeys = 0
    scapekey = 0
    twopressedkeys = 0
    mouseclicks = 0
    euclideandistance = 0
    mousewheel = 0
    size = 0
    meanPressedKeys = 0
    meanScapekey = 0
    meanTwopressedkeys = 0
    meanMouseclicks = 0
    meanEuclideandistance = 0
    meanMousewheel = 0

    pressedKeysAlarm = 0
    scapekeyAlarm = 0
    twopressedkeysAlarm = 0
    mouseclicksAlarm = 0
    euclideandistanceAlarm = 0
    mousewheelAlarm = 0
    listMean = [0.0,0.0,0.0,0.0,0.0,0.0]
    listAlarm = [0.0,0.0,0.0,0.0,0.0,0.0]
    listFinalAlarm = [0.0,0.0,0.0,0.0,0.0,0.0]
    listAlarm = [0.0,0.0,0.0,0.0,0.0,0.0]

    listMean = json_read(listMean)
    listAlarm = json_alarm(listAlarm)
    listFinalAlarm = set_alarms(listMean,listAlarm,listFinalAlarm)
    ax = draw_grafic_keyboard(listFinalAlarm,ax)
    ax = draw_grafic_mouse(listFinalAlarm,ax)

listMean = [0.0,0.0,0.0,0.0,0.0,0.0]
listAlarm = [0.0,0.0,0.0,0.0,0.0,0.0]
listFinalAlarm = [0.0,0.0,0.0,0.0,0.0,0.0]
listAlarm = [0.0,0.0,0.0,0.0,0.0,0.0]
ax = plt.subplot(111, polar=True)
main_alarms(listMean,listAlarm,ax,listFinalAlarm)
