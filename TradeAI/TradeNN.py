import math
import matplotlib.pyplot as plt
import matplotlib
import numpy as np
import tensorflow as tf

from stock import loaddata
from network import *

def generatedata():

    NB_STEP = 720*5

    data = np.ones(NB_STEP)
    time = np.ones(NB_STEP)
    
    for i in range(0,NB_STEP,1):
        angle = (math.pi * i * 2) / 180.0
        angle1 = 0.1*angle
        angle2 = .63*angle
        angle3 = 1.6*angle
        data[i]= math.sin(angle1) * 20 + math.cos(angle2) * 9 + math.cos(angle3) * 3 + 60
        time[i]=i

    return data, time

# Calculate data
#close, x = generatedata()
close, x = loaddata("data/goog.txt")

# Calculate EMA
ema1 = ema(close, 6)
ema2 = ema(close, 20)

# Calcule OSC
osc1 = (close - ema1)/close
osc2 = (ema1 - ema2)/ema1

fig = plt.figure()
plt.gcf().canvas.set_window_title("Q-Learn")

ax1 = fig.add_subplot(211)
ax1.plot(x, close, label="close")
#ax1.plot(x, ema1, label="ema1")
#ax1.plot(x, ema2, label="ema2")


ax2 = fig.add_subplot(212)
ax2.plot(x, osc1, label="osc1")
ax2.plot(x, osc2, label="osc2")

#value, trades = evaluate()

#dates = np.stack((t.date for t in trades if t.kind == "buy"))
#values = np.stack((t.value for t in trades if t.kind == "buy"))
#ax1.plot(dates, values, 'go')

#dates = np.stack((t.date for t in trades if t.kind == "sell"))
#values = np.stack((t.value for t in trades if t.kind == "sell"))
#ax1.plot(dates, values, 'ro')


sample = 20
sampling = 20

nbinput=sample
nboutput=2

#train_step = createnetwork(sampling, 2)
 # input X: nbinput normalized, the first dimension (None) will index the samples in the mini-batch
X = tf.placeholder(tf.float32, [None, nbinput])
# correct answers will go here
Y_ = tf.placeholder(tf.float32, [None, nboutput])
# weights W[nbinput, nboutput]
W = tf.Variable(tf.zeros([nbinput, nboutput]))
# biases b[nboutput]
b = tf.Variable(tf.zeros([nboutput]))
    
XX = tf.reshape(X, [-1, nbinput])

# The model
Y = tf.nn.softmax(tf.matmul(XX, W) + b)

# loss function: cross-entropy = - sum( Y_i * log(Yi) )
#                           Y: the computed output vector
#                           Y_: the desired output vector

# cross-entropy
# log takes the log of each element, * multiplies the tensors element by element
# reduce_mean will add all the components in the tensor
# so here we end up with the total cross-entropy for all images in the batch
cross_entropy = -tf.reduce_mean(Y_ * tf.log(Y)) * 1000.0  # normalized for batches of 100 images,
                                                            # *10 because  "mean" included an unwanted division by 10

# accuracy of the trained model, between 0 (worst) and 1 (best)
correct_prediction = tf.equal(tf.argmax(Y, 1), tf.argmax(Y_, 1))
accuracy = tf.reduce_mean(tf.cast(correct_prediction, tf.float32))

# training, learning rate = 0.005
train_step = tf.train.GradientDescentOptimizer(0.005).minimize(cross_entropy)

# init
init = tf.global_variables_initializer()
sess = tf.Session()
sess.run(init)

start = sample
end = len(close) - sample

batchsize = 5
batch_X = np.ones((batchsize,sample))
batch_Y = np.ones((batchsize,2))

index = 0
for i in range(start, end):
    if (close[i+sampling] > close[i]):
        batch_X[index] = osc1[i-sampling:i]
        batch_Y[index][0] = 1.0
        batch_Y[index][1] = 0.0
    else:
        batch_X[index] = osc1[i-sampling:i]
        batch_Y[index][0] = 0.0
        batch_Y[index][1] = 1.0

    index=index+1
    if (index==batchsize):
        sess.run(train_step, feed_dict={X: batch_X, Y_: batch_Y})
        index = 0

        y = sess.run(Y, feed_dict={X: batch_X})
        print (y)


plt.show()


