import tensorflow as tf

def createnetwork(nbinput, nboutput):
    
    # input X: nbinput normalized, the first dimension (None) will index the samples in the mini-batch
    X = tf.placeholder(tf.float32, [None, nbinput, 1])
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

    return train_step

