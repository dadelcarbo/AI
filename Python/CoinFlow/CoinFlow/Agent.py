import keras
from keras.models import Sequential
from keras.models import load_model
from keras.layers import Dense
from keras.optimizers import Adam

import numpy as np
import random
from collections import deque

class Agent:
	def __init__(self, state_size, is_eval=False, model_name=""):
		self.state_size = state_size # normalized previous days
		self.position = int(state_size/2)+1
		self.action_size = 3 # nop, right, sell
		self.memory = deque(maxlen=1000)
		self.inventory = []
		self.model_name = model_name
		self.is_eval = is_eval

		self.gamma = 0.95
		self.epsilon = 1.0
		self.epsilon_min = 0.01
		self.epsilon_decay = 0.995

		self.model = self._model()

		#self.model = load_model("models/" + model_name) if is_eval else
		#self._model()

	def _model(self):
		model = Sequential()
		model.add(Dense(units=self.state_size, input_dim=self.state_size, activation="relu"))
		model.add(Dense(self.action_size, activation="linear"))
		model.compile(loss="mse", optimizer=Adam(lr=0.001))

		return model

	def act(self, state):
		if not self.is_eval and np.random.rand() <= self.epsilon:
			return random.randrange(self.action_size)

		options = self.model.predict(state)
		return np.argmax(options[0])

	def move(self, dir):		
		if dir == 1:    #right
		    self.position = min(self.position+1, self.state_size -1)
		elif dir == 2:     #left        
		    self.position = max(self.position-1, 0)
		return self.position

	def expReplay(self, batch_size):
		mini_batch = []
		l = len(self.memory)
		for i in range(l - batch_size + 1, l):
			mini_batch.append(self.memory[i])

		for state, action, reward, next_state, done in mini_batch:
			target = reward
			if not done:
				target = reward + self.gamma * np.amax(self.model.predict(next_state)[0])

			target_f = self.model.predict(state)
			target_f[0][action] = target
			self.model.fit(state, target_f, epochs=1, verbose=0)

		if self.epsilon > self.epsilon_min:
			self.epsilon *= self.epsilon_decay 

	def train(self, ground, epoc, length, height, batch_size):
		#loop through ground by pack of width
		for e in range(epoc):
			r = 0
			for i in range(length - height):
				state = ground[range(i,i + height),:]

				action = self.act(state)

				self.move(action)

				if state[-1,self.position] == 1:
				    r+=1
				else:
				    r -= 0.1

				self.memory.append((state, action, r, state, False))
				if len(self.memory) > batch_size:
				    self.expReplay(batch_size)

				return r
