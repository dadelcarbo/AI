import  math
import matplotlib.pyplot as plt
import matplotlib
import random

import numpy as np

from trade import *
import stock

# Actions
buy = 0
sell = 1
wait = 2

# Agent State
flat = 0
long = 1

# Environment State
rising = 0
falling = 1

QTarget = np.zeros((2,2,3))

QTarget[flat][rising][buy] = 1
QTarget[long][rising][buy] = -1
QTarget[flat][falling][buy] = 0
QTarget[long][falling][buy] = -1
QTarget[flat][rising][sell] = -1
QTarget[long][rising][sell] = 0
QTarget[flat][falling][sell] = -1
QTarget[long][falling][sell] = 1
QTarget[flat][rising][wait] = 0
QTarget[long][rising][wait] = 1
QTarget[flat][falling][wait] = 0
QTarget[long][falling][wait] = 0

Q = QTarget

def evaluate():

    state = flat
    value = 0
    entry = 0
    
    trades = [trade]

    for i in range(1, len(close)):
        
        trend = int(close[i] < close[i - 1])

        action = -1
        best = 0
        for j in [buy, sell, wait]:
            if (Q[state][trend][j] > best):
                best = Q[state][trend][j]
                action = j
                break

        if (action == buy):
            entry = close[i]
            state = long
            trades.append(trade(i, close[i], "buy"))
        elif (action == sell):
            value = value + close[i] - entry
            state = flat
            trades.append(trade(i, close[i], "sell"))

    if (state == long):
        value = value + close[len(close) - 1]
    
    return value,trades

def evaluate_MC():

    state = flat
    states = []    
    states.append(flat)
    value = 0
    values = []    
    values.append(0)

    entry = 0
    
    trades = []
    
    trend = rising
    
    for i in range(1, len(close)):

        if (state == flat):
            values.append(value)
        else:
            values.append(value + close[i] - entry)

        rnd = random.randint(0,100)
        if (rnd > 97):
            if (trend == rising): 
                trend = falling
            else: 
                trend = rising

#        if (close[i - 1] < close[i]):
#            trend = rising
#        else:
#            trend = falling

        action = -1
        best = 0
        for j in [buy, sell, wait]:
            if (Q[state][trend][j] > best):
                best = Q[state][trend][j]
                action = j
                break

        if (action == buy):
            entry = close[i]
            state = long
            trades.append(trade(i, close[i], "buy"))
        elif (action == sell):
            value = value + close[i] - entry
            state = flat
            trades.append(trade(i, close[i], "sell"))
        states.append(state)
   
    return states, values,trades

def generatedata(size = 5000, offset = 100,sigma=2):
    data = []
    time = []
    
    for i in range(0,size,1):
        if (sigma!=0):
            offset = offset + random.normalvariate(0, sigma)
        angle = (math.pi * i * 4) / 180.0
        angle1 = 2 * angle
        angle2 = 0.55 * angle
        data.insert(0,math.sin(angle1) * 10 + math.cos(angle2) * 20 + offset)
        time.append(i)
        offset = offset + random.normalvariate(0, sigma)

    return data, time

close, x = generatedata()
#close, x = stock.loaddata("data/goog.txt")

bestValue = -100000
bestTrades = []
bestValues = []
bestStates = []

for i in range(0,10000):
    state, value, trades = evaluate_MC()
    if (value[-1] > bestValue):
        bestValue = value[-1]
        bestTrades = trades
        bestValues = value
        bestStates = state
        print(bestValue)
        
fig = plt.figure()
plt.gcf().canvas.set_window_title("Q-Learn")

ax1 = fig.add_subplot(311)
ax1.set_xlim(0, len(close))
ax1.plot(x, close, label="close")

oscil = stock.osc(close, 20, 60)

ax2 = fig.add_subplot(312)
ax2.set_xlim(0, len(close))
ax2.plot(x, oscil, label="Portfolio")

oscil = stock.ror(close, 100)

ax3 = fig.add_subplot(313)
ax3.set_xlim(0, len(close))
ax3.plot(x, oscil, label="OSC")

dates = np.stack((t.date for t in bestTrades if t.kind == "buy"))
values = np.stack((t.value for t in bestTrades if t.kind == "buy"))
ax1.plot(dates, values, 'go')

dates = np.stack((t.date for t in bestTrades if t.kind == "sell"))
values = np.stack((t.value for t in bestTrades if t.kind == "sell"))
ax1.plot(dates, values, 'ro')

plt.show()
