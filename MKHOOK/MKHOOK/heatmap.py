import tkinter
import matplotlib.pyplot as plt
from matplotlib.colors import LinearSegmentedColormap
import numpy as np
from time import localtime, strftime

import scipy.ndimage.filters as filters

p=tkinter.Tk()
p.withdraw()
width = p.winfo_screenwidth()
height = p.winfo_screenheight()
print(p.winfo_pointerxy())
print(width)
print(height)

def plot(data, save_path):
    colors = [(0, 0, 1), (0, 1, 1), (0, 1, 0.75), (0, 1, 0), (0.75, 1, 0),
              (1, 1, 0), (1, 0.8, 0), (1, 0.7, 0), (1, 0, 0)]

    cm = LinearSegmentedColormap.from_list('sample', colors)

    plt.imshow(data, cmap=cm)
    plt.axis('off')
    plt.tight_layout()
    plt.savefig(save_path)
    plt.close()
if __name__ == "__main__":
    w = width
    h = height
    data = np.zeros(h * w)
    data = data.reshape((h, w))
    while True:
        x,y = p.winfo_pointerxy()
        data = data.reshape((h, w))
        data[y][x] = data[y][x]+1   
        fileTime = strftime("%Y-%m-%d %H-%M-%S", localtime())
        plot(filters.gaussian_filter(data, sigma=50), 'heatmap/heatmap'+fileTime+'.png')