import numpy as np

coinRatio = 0.25

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

width = 5
length = 10
#makeFile(10, length, width)

ground = makeGround(length, width)
print("ground")
print(ground)


#loop through ground by pack of 5
for i in range(length - width):
    screen = ground[range(i,i+width),:]
    print("screen "+str(i))
    print(screen)

