import sys
from Tkinter import *
from twisted.internet import reactor, tksupport
from twisted.python import log
from autobahn.websocket import WebSocketServerFactory, WebSocketServerProtocol, listenWS
import random
 
class BroadcastServerProtocol(WebSocketServerProtocol):
 
    def onOpen(self):
        self.factory.register(self)

    def onMessage(self, msg, binary):
        if not binary:
            self.factory.broadcast("received message '%s' from %s" % (msg, self.peerstr))

    def connectionLost(self, reason):
        WebSocketServerProtocol.connectionLost(self, reason)
        self.factory.unregister(self)
 
 
class BroadcastServerFactory(WebSocketServerFactory): 
    protocol = BroadcastServerProtocol

    def __init__(self, url):
      WebSocketServerFactory.__init__(self, url)
      self.clients = []
      self.tickcount = 0
      self.maxWidth = 700
      self.maxHeight = 500
      self.leftHandPosition = [0, 0]
      self.rightHandPosition= [0, 0]
      self.leftHandState = "O"
      self.rightHandState = "O"
      self.deltaScale = 5
      self.broadcast("Let's start!")
      #self.tick()

    def incrementHandPosition(self, Hand, deltaX, deltaY):
        if (Hand =="left"):
            handHandle = self.leftHandPosition
        else:
            handHandle = self.rightHandPosition
        
        handHandle[0] = max( 0, min( handHandle[0] + deltaX* self.deltaScale, self.maxWidth) )
        handHandle[1] = max( 0, min( handHandle[1] + deltaY* self.deltaScale, self.maxHeight) )
        
    def handChangeState(self, Hand, openOrClose):
        if Hand=="L":
            self.leftHandState = openOrClose
        else:
            self.rightHandState = openOrClose
    
    def broadcastHandPosition(self):
        print "Left hand (%d, %d)" % (self.leftHandPosition[0], self.leftHandPosition[1])
        print "Right hand (%d, %d)" % (self.rightHandPosition[0], self.rightHandPosition[1])
        self.broadcast("L(%d, %d)" % (self.leftHandPosition[0], self.leftHandPosition[1]) )
        self.broadcast("R(%d, %d)" % (self.rightHandPosition[0], self.rightHandPosition[1]) )
    
    def broadcastHandState(self):
        print "S(L,%s)" % (self.leftHandState)
        print "S(R,%s)" % (self.rightHandState)
        self.broadcast("S(L,%s)" % (self.leftHandState))
        self.broadcast("S(R,%s)" % (self.rightHandState))

    def tick(self):
      self.tickcount += 1
      #self.broadcast("tick %d" % self.tickcount)
      
      x = random.randint(0, self.maxWidth)
      y = random.randint(0, self.maxHeight)
      newMsg= '%d,%d' % (x, y)
      self.broadcast(newMsg)
      reactor.callLater(10, self.tick)

    def register(self, client):
        if not client in self.clients:
            print "registered client " + client.peerstr
            self.clients.append(client)

    def unregister(self, client):
        if client in self.clients:
            print "unregistered client " + client.peerstr
            self.clients.remove(client)

    def broadcast(self, msg):
        print "broadcasting message '%s' .." % msg
        for c in self.clients:
             print "send to " + c.peerstr
             c.sendMessage(msg)
 
def key(event):
    changePosition = False
    changeOpenCloseState = False
    if event.keysym == 'Escape':
        reactor.stop()
        
        #root.destroy()
    elif event.keysym == 'Left':
        factory.incrementHandPosition('right', -1, 0)
        changePosition = True
    elif event.keysym == 'Right':
        factory.incrementHandPosition('right', 1, 0)            
        changePosition = True
    elif event.keysym == 'Down':
        factory.incrementHandPosition('right', 0, 1)
        changePosition = True
    elif event.keysym == 'Up':
        factory.incrementHandPosition('right', 0, -1)                    
        changePosition = True
    elif event.keysym == 'a':
        factory.incrementHandPosition('left', -1, 0)
        changePosition = True
    elif event.keysym == 'd':
        factory.incrementHandPosition('left', 1, 0)            
        changePosition = True
    elif event.keysym == 's':
        factory.incrementHandPosition('left', 0, 1)
        changePosition = True
    elif event.keysym == 'w':
        factory.incrementHandPosition('left', 0, -1)                               
        changePosition = True
    elif event.keysym == 'q':
        factory.handChangeState("L", "O")
        changeOpenCloseState = True
    elif event.keysym == 'e':
        factory.handChangeState("L", "C")
        changeOpenCloseState = True
    elif event.keysym == 'comma':
        factory.handChangeState("R", "O")
        changeOpenCloseState = True
    elif event.keysym == 'period':
        factory.handChangeState("R", "C")
        changeOpenCloseState = True


    print event.char, event.keysym
    if changePosition:
        factory.broadcastHandPosition() 
    elif changeOpenCloseState:
        factory.broadcastHandState()

if __name__ == '__main__':
    root = Tk()
    # Install the Reactor support
    tksupport.install(root)
    print "Press a key (Escape key to exit):"
    root.bind_all('<Key>', key)
    # don't show the tk window
    #root.withdraw()
    
    log.startLogging(sys.stdout)
    factory = BroadcastServerFactory("ws://localhost:9000")
    listenWS(factory)
    reactor.run()
