import numpy as np
import csv

def loaddata(filename):
    with open(filename, mode="r") as csvfile:
        reader = csv.DictReader(csvfile)
        close = []
        for row in reader:
            close.insert(0,float(row['Close']))

    data = np.asarray(close)
    dates = np.asarray(range(0,len(close)))
    return close, dates

def osc(values, fastPeriod, slowPeriod):
    fastEMA = ema(values, fastPeriod)
    slowEMA = ema(values, slowPeriod)
    return (fastEMA - slowEMA) / slowEMA

def ema(values, period):
    alpha = 2.0 / (float)(period + 1)
    ema = np.ones(len(values))
    ema[0] = values[0]
    for i in range(1,len(values)):
        ema[i] = ema[i - 1] + alpha * (values[i] - ema[i - 1])
    return ema

def stock(values, period):
    st = []
    st.append(0)
    for i in range(1, len(values)):
        subarray = values[max(0, i - period): i+1]
        mn = min(subarray)
        mx = max(subarray)
        val = (values[i] - mn) / (mx - mn)
        st.append(val * 2 - 1)
    return st

def ror(values, period):
    r = []
    r.append(0)
    for i in range(1, len(values)):
        subarray = values[max(0, i - period): i+1]
        mn = min(subarray)
        mx = max(subarray)
        val = (values[i] - mn) / mn
        r.append(val * 2 - 1)
    return r
    