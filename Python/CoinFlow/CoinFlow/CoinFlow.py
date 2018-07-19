import numpy as np
from Agent import Agent

coinRatio = 0.1

def toCoin(v):
    if (v < coinRatio):
        return 1
    else:
        return 0

def lineToString(count):
    line = np.random.rand(count)
    res = ""
    for v in line:
        res += str(toCoin(v))
    return res

def makeFile(nbFile, length, width):
    for k in range(nbFile):
        text_file = open("data/ground" + str(k) + ".txt", "w")
        print(width, file=text_file)

        for i in range(length):
            line = lineToString(width)
            text_file.write(line + "\n")
        text_file.close()

def makeGround(length, width):
    x = np.random.rand(length, width)
    y = np.zeros((length,width))
    for i in range(length):
        for j in range(width):
                y[i][j] = toCoin(x[i][j])
    return y

def plot(ax, state, agent):
    A = state
    ax.imshow(A, interpolation='nearest')
    plt.show()

width = 5
length = 100
#makeFile(10, length, width)
ground = makeGround(length, width)
print("ground")
print(ground)
height = 1
print(range(length - height))

agent = Agent(width, is_eval = True)

reward = 0

actions = ['nop', 'right', 'left']

batch_size = 10

#def train():
#    #loop through ground by pack of width
#    r = 0
#    for i in range(length - height):
#        state = ground[range(i,i + height),:]
#        #print(state)
#        #visu = np.zeros((1,width))
#        #visu[0, agent.position] = 1
#        #print(visu)
#        action = agent.act(state)
#        #print("Action: " + actions[action])

#        agent.move(action)

#        if state[-1,agent.position] == 1:
#            r+=1
#        else:
#            r -= 0.1

#        #print("Pos: " + str(agent.position) + " Reward: " + str(reward))

#        agent.memory.append((state, action, r, state, False))
#        if len(agent.memory) > batch_size:
#            agent.expReplay(batch_size)

#    return r
def evaluate():
    #loop through ground by pack of width
    r = 0
    for i in range(length - height):
        state = ground[range(i,i + height),:]
        #print(state)
        
        action = agent.act(state)
       
        agent.move(action)

        if state[-1,agent.position] == 1:
            r+=1
        else:
            r -= 0.1

    return r

for i in range(100):
    print("Step: " + str(i) + "Reward: " + str(agent.train(ground, 10, length, height, batch_size)))

print("Final Reward: " + str(evaluate()))