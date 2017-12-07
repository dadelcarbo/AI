from tkinter import *
import math
import matplotlib.pyplot as plt
from matplotlib.backends.backend_tkagg import FigureCanvasTkAgg
import matplotlib
import random
from threading import Timer

import numpy as np

from trade import *
import stock

isStarted = False

def startCallBack():
    global isStarted
    global startbutton
    global stopbutton
    global timer
    isStarted = True
    startbutton.configure(state=DISABLED)
    stopbutton.configure(state=NORMAL)
    timer = Timer(0.1, step)
    timer.start() 

   
def stopCallBack():
    global isStarted
    global startbutton
    global stopbutton
    global timer
    isStarted = False
    startbutton.configure(state=NORMAL)
    stopbutton.configure(state=DISABLED)
    timer.cancel()

def step():
    global close
    close.append(random.randint(0,100))
    global timer
    timer = Timer(0.1, step)
    timer.start()
    global ax1
    ax1.set_xlim(0, len(close))
    ax1.plot(range(len(close)), close, label="close")
    global fig
    fig.canvas.draw()
    
root = Tk()
frame = Frame(root)
frame.pack()

bottomframe = Frame(root)
bottomframe.pack(side = BOTTOM)

startbutton = Button(frame, text="Start", command = startCallBack)
startbutton.pack(side = LEFT)
stopbutton = Button(frame, text="Stop", command = stopCallBack)
stopbutton.configure(state=DISABLED)
stopbutton.pack(side = LEFT)

close = [0,1,2,3]

fig = plt.figure()
ax1 = fig.add_subplot(111)
ax1.set_ylabel('Close', color='g')
ax1.set_xlim(0, len(close)-1)
ax1.plot(range(len(close)), close, label="close")
 
graph = FigureCanvasTkAgg(fig, master=bottomframe)
canvas = graph.get_tk_widget()
canvas.grid(row=0, column=0)

root.mainloop()