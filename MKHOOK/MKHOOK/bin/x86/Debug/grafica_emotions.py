# Libraries
import matplotlib.pyplot as plt
import pandas as pd
from math import pi


# Set data
df = pd.DataFrame({
'group': ['anger','fear','joy','sadness'],
'anger': [38, 0, 0, 0],
'fear': [29, 0, 0, 0],
'joy': [8, 0, 0, 0],
'sadness': [7, 0, 0, 0]
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
 
# Initialise the spider plot
ax = plt.subplot(111, polar=True)
 
# Draw one axe per variable + add labels labels yet
plt.xticks(angles[:-1], categories, color='grey', size=8)
 
# Draw ylabels
ax.set_rlabel_position(0)
plt.yticks([ 0, 10, 20, 30, 40, 50, 60, 70, 80, 90, 100], ["0", "10", "20", "30", "40", "50", "60", "70", "80", "90", "100"], color="grey", size=7)
plt.ylim(0,100)
 
# Plot data
ax.plot(angles, values, linewidth=1, linestyle='solid')
 
# Fill area
ax.fill(angles, values, 'b', alpha=0.1)

plt.savefig('emotions chart.png')
