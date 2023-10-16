import numpy as np
import matplotlib.pyplot as plt
import pandas as pd
import seaborn as sn
import math
from matplotlib import collections  as mc
import pylab as pl
import time
import keyboard




# Movement controls:
#           U,
#        H, J, k

# Avoid the falling lines! Take the pentagons before you collapse to death!

on = True
width = 300
length = 300


deltaY = 200
t1, t2 = 0, 0
fps = 60
wait = 1/fps
spawnTime = 1
playerDecayPeriod = 0.1
playerLength = 20
losingLength = 5
lengthDecayRate = 1.5
maxPlayerLength = 80
lengthIncrement = 5
r = 75
X = []
Y = []

appleX = 50
appleY = 50

score = 0

deltaTheta = 0.2
speed = 12.5
playerX = [width/2, width/2]
playerY = [length/2, length/2 + playerLength]

colors = ['r', 'g', 'b', 'c', 'm', 'y', 'k']

def randomAngle():
    return float(np.random.random() * 2 * math.pi)

def spawn():

    x1 = np.random.random() * width
    x2 = x1 + r * math.cos(randomAngle())
    y1 = 350
    y2 = y1 + r * math.sin(randomAngle())

    if x2 > x1:
        X.append(x1)
        X.append(x2)
        Y.append(y1)
        Y.append(y2)
    else:
        X.append(x2)
        X.append(x1)
        Y.append(y2)
        Y.append(y1)


def destroy():
    for i in range(int(len(Y)/2)):
        if(Y[2*i] < 0 and Y[2*i + 1] < 0):
            del X[2*i + 1]
            del X[2*i]
            del Y[2*i + 1]
            del Y[2*i]

            break

def updatePlayerLength(delta):
    global playerX
    global playerY
    global playerLength
    if(playerLength < maxPlayerLength):
        playerLength += delta
        dY = playerY[1] - playerY[0]
        dX = playerX[1] - playerX[0]
        displacment = [(playerY[1] - playerY[0])/math.sqrt(dY*dY + dX*dX), (playerX[1] - playerX[0])/math.sqrt(dY*dY + dX*dX)]

        playerX[0] = playerX[0] - displacment[1] * delta / math.sqrt(2)
        playerX[1] = playerX[1] + displacment[1] * delta / math.sqrt(2)
        playerY[0] = playerY[0] - displacment[0] * delta / math.sqrt(2)
        playerY[1] = playerY[1] + displacment[0] * delta / math.sqrt(2)


def rotatePlayer(deltaTheta):
    global playerX
    global playerY

    mX = (playerX[0] + playerX[1])/2
    mY = (playerY[0] + playerY[1])/2
    playerXN = [playerX[0] - mX, playerX[1] -mX]
    playerYN = [playerY[0] - mY, playerY[1] -mY]

    if(playerXN[0] == 0 and playerYN[0] > 0):
        oldAngle = math.pi / 2
    elif(playerXN[0] == 0 and playerYN[0] < 0):
        oldAngle = 3 * math.pi / 2
    elif(playerXN[0] < 0 and playerXN[0] > 0):
        oldAngle = math.atan(playerYN[0]/ playerXN[0]) + math.pi
    elif(playerXN[0] < 0 and playerXN[0] < 0):
        oldAngle = math.atan(playerYN[0]/ playerXN[0]) + math.pi
    else:
        oldAngle = math.atan(playerYN[0]/ playerXN[0])

    newAngle1 = oldAngle + deltaTheta
    newAngle2 = newAngle1 + math.pi

    playerX = [mX + (playerLength/2) * math.cos(newAngle1), mX + (playerLength/2) * math.cos(newAngle2)]
    playerY = [mY + (playerLength/2) * math.sin(newAngle1), mY + (playerLength/2) * math.sin(newAngle2)]

def translatePlayer(delta):
    global playerX
    global playerY
    
    dY = playerY[1] - playerY[0]
    dX = playerX[1] - playerX[0]
    displacment = [(playerY[1] - playerY[0])/math.sqrt(dY*dY + dX*dX), (playerX[1] - playerX[0])/math.sqrt(dY*dY + dX*dX)]

    playerX[0] = playerX[0] + displacment[1] * delta
    playerX[1] = playerX[1] + displacment[1] * delta
    playerY[0] = playerY[0] + displacment[0] * delta
    playerY[1] = playerY[1] + displacment[0] * delta


def paramatrizeSegment(points):
    x1 = points[0][0]
    y1 = points[0][1]
    x2 = points[1][0]
    y2 = points[1][1]

    if x1 > x2:
        tempX = x2
        tempY = y2
        x2 = x1
        y2 = y1
        x1 = tempX
        y1 = tempY
    
    m = (y2 - y1) / (x2 - x1)
    c = y2 - m * x2

    return [m, c]

def lost():
    global on
    on = False
    plt.text(width/2 - width/3, length/2, "        You Lost! \n Your Final Score is " + str(score) + "!!", fontsize=22)

def rotateVector(vector, center, delta):
    vecterN = [vector[0] - center[0], vector[1] - center[1]]
    magnitude = math.sqrt(vecterN[0] * vecterN[0] + vecterN[1] * vecterN[1])
    oldAngle = 0
    if(vecterN[0] == 0 and vecterN[1] > 0):
        oldAngle = math.pi / 2
    elif(vecterN[0] == 0 and vecterN[1] < 0):
        oldAngle = 3 * math.pi / 2
    elif(vecterN[0] < 0 and vecterN[0] > 0):
        oldAngle = math.atan(vecterN[1]/ vecterN[0]) + math.pi
    elif(vecterN[0] < 0 and vecterN[0] < 0):
        oldAngle = math.atan(vecterN[1]/ vecterN[0]) + math.pi
    else:
        oldAngle = math.atan(vecterN[1]/ vecterN[0])

    newAngle = oldAngle + delta
    return [center[0] + magnitude * math.cos(newAngle), center[0] + magnitude * math.sin(newAngle)]

spawn()
while(on == True):
    t = time.perf_counter()

    i = 0
    
    if t - t1 > spawnTime:
        t1 = t
        spawn()

    if t - t2 > spawnTime:
        t2 = t
        updatePlayerLength(-lengthDecayRate)

    while i < len(X)/2:
        plt.plot([X[2*i],X[2*i + 1]],[Y[2*i],Y[2*i + 1]], colors[int(np.random.random() * len(colors))])
        i += 1

    for i in range(len(Y)):
        Y[i] -= deltaY * wait

    plt.plot(playerX, playerY)
    plt.plot(playerX[0], playerY[0], 'o', markersize = 6, markeredgecolor="red", markerfacecolor="red")
    plt.plot(playerX[1], playerY[1], 'o', markersize = 6, markeredgecolor="green", markerfacecolor="green")

    scoreLabel = "score: " + str(score)
    plt.plot(appleX,appleY, 'H', markersize = 10, label = scoreLabel, markeredgecolor="purple", markerfacecolor="purple")
    plt.legend()
    plt.legend(loc='upper right')


    distanceFromApple1 = math.sqrt(math.pow((playerX[1] - appleX), 2) + math.pow((playerY[1] - appleY), 2))
    distanceFromApple0 = math.sqrt(math.pow((playerX[0] - appleX), 2) + math.pow((playerY[0] - appleY), 2))

    if(distanceFromApple1 < 10 or distanceFromApple0 < 10):
        appleX = np.random.random() * width / 2 + width / 4
        appleY = np.random.random() * length / 2 + length / 8
        score += 10
        updatePlayerLength(lengthIncrement)


    for i in range(int((len(X)/2))):
        m, c = paramatrizeSegment([[X[2*i], Y[2*i]],[X[2*i + 1], Y[2*i + 1]]])
        valid = False
        if playerY[0] < m * playerX[0] + c and playerY[1] > m * playerX[1] + c:
            x1 = playerX[0]
            y1 = playerY[0]
            x2 = playerX[1]
            y2 = playerY[1]
            valid = True

        elif playerY[0] > m * playerX[0] + c and playerY[1] < m * playerX[1] + c:
            x1 = playerX[1]
            y1 = playerY[1]
            x2 = playerX[0]
            y2 = playerY[0]
            valid = True


        if valid:

            m1, c1 = paramatrizeSegment([[x1, y1],[X[2*i], Y[2*i]]])
            x = np.linspace(0,width,100)
            y = m1*x+c1
            # plt.plot(x, y, 'r', '-r', label='y=2x+1')
            m2, c2 = paramatrizeSegment([[x1, y1],[X[2*i + 1], Y[2*i + 1]]])
            x = np.linspace(0,width,100)
            y = m2*x+c2
            # plt.plot(x, y, 'b','-r', label='y=2x+1')
            d = np.linspace(0,width,width)

            x,y = np.meshgrid(d,d)

            # if (m2 - m) > 0:
            #     if(m1 - m) > 0:
            #         plt.imshow( ((y < m1 * x + c1) & (y > m2 * x + c2)& (y>m * x + c)).astype(int))
            #     else:
            #         plt.imshow( ((y > m1 * x + c1) & (y > m2 * x + c2)& (y>m * x + c)).astype(int))                    
            # else:
            #     if(m1 - m) > 0:
            #         plt.imshow( ((y < m1 * x + c1) & (y < m2 * x + c2)& (y>m * x + c)).astype(int))
            #     else:
            #         plt.imshow( ((y > m1 * x + c1) & (y < m2 * x + c2)& (y>m * x + c)).astype(int))            

            m2, c2 = paramatrizeSegment([[x1, y1],[X[2*i + 1], Y[2*i + 1]]])
            if (m2 - m) > 0:
                m1, c1 = paramatrizeSegment([[x1, y1],[X[2*i], Y[2*i]]])
                if(m1 - m) > 0:
                    if((y2 < m1 * x2 + c1) and (y2 > m2 * x2 + c2)):
                        lost()
                        break
                else:
                    if((y2 > m1 * x2 + c1) and (y2 > m2 * x2 + c2)):
                        lost()
                        break       
            else:
                m1, c1 = paramatrizeSegment([[x1, y1],[X[2*i], Y[2*i]]])
                if(m1 - m) > 0:
                    if((y2 < m1 * x2 + c1) and (y2 < m2 * x2 + c2)):
                        lost()
                        break        
                else:
                    if((y2 > m1 * x2 + c1) and (y2 < m2 * x2 + c2)):
                        lost()
                        break          

    if playerLength < losingLength:
        lost()
    if keyboard.is_pressed("h"):
        rotatePlayer(deltaTheta)
    elif keyboard.is_pressed("k"):
        rotatePlayer(-deltaTheta)

    # if keyboard.is_pressed("z"):
    #     center = [(X[0] + X[1])/2, (Y[0] + Y[1])/2]
    #     X[0], Y[0] = rotateVector([X[0], Y[0]], center, deltaTheta)
    #     X[1], Y[1] = rotateVector([X[1], Y[1]], center, deltaTheta)
    # elif keyboard.is_pressed("x"):
    #     center = [(X[0] + X[1])/2, (Y[0] + Y[1])/2]
    #     X[0], Y[0] = rotateVector([X[0], Y[0]], center, -deltaTheta)
    #     X[1], Y[1] = rotateVector([X[1], Y[1]], center, -deltaTheta)

    if keyboard.is_pressed("u"):
        translatePlayer(speed)


    elif keyboard.is_pressed("j"):
        translatePlayer(-speed)


    destroy()
    plt.pause(wait)
    if(on):
        plt.clf()
    plt.xlim([0, width])
    plt.ylim([0, length])

plt.show()