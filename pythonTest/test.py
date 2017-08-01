import cv2
import numpy as np
import os, sys

im = cv2.imread('pic.png',flags = 0)
retval, im = cv2.threshold(im, 115, 255, cv2.THRESH_BINARY_INV)
for i in xrange(len(im)):
	for j in xrange(len(im[i])):
		if im[i][j] == 255:
			count = 0 
			for k in range(-2, 3):
				for l in range(-2, 3):
					try:
						if im[i + k][j + l] == 255:
							count += 1
					except IndexError:
						pass
			
			if count <= 4:
				im[i][j] = 0
					
im = cv2.dilate(im, (2, 2), iterations=1)	

image,contours, hierarchy = cv2.findContours(im.copy(), cv2.RETR_TREE, cv2.CHAIN_APPROX_SIMPLE)
cnts = sorted([(c, cv2.boundingRect(c)[0]) for c in contours], key=lambda x:x[1])

arr = []

for index, (c, _) in enumerate(cnts):
	(x, y, w, h) = cv2.boundingRect(c)

	try:
			
		if w > 8 and h > 8:
			add = True
			for i in range(0, len(arr)):
					
				if abs(cnts[index][1] - arr[i][0]) <= 3:
					add = False
					break
			if add:
				arr.append((x, y, w, h))
	except IndexError:
		pass

for index, (x, y, w, h) in enumerate(arr):
	roi = im[y: y + h, x: x + w]
	thresh = roi.copy() 
	
	angle = 0
	smallest = 999
	row, col = thresh.shape

	for ang in range(-60, 61):
		M = cv2.getRotationMatrix2D((col / 2, row / 2), ang, 1)
		t = cv2.warpAffine(thresh.copy(), M, (col, row))

		r, c = t.shape
		right = 0
		left = 999

		for i in xrange(r):
			for j in xrange(c):
				if t[i][j] == 255 and left > j:
					left = j
				if t[i][j] == 255 and right < j:
					right = j

		if abs(right - left) <= smallest:
			smallest = abs(right - left)
			angle = ang

	M = cv2.getRotationMatrix2D((col / 2, row / 2), angle, 1)
	thresh = cv2.warpAffine(thresh, M, (col, row))
	thresh = cv2.resize(thresh, (50, 50))

	cv2.imwrite('tmp/' + str(index) + '.png', thresh)

def mse(im1, im2):
	err = np.sum((im1.astype('float') - im2.astype('float')) ** 2)
	err /= float(im1.shape[0] * im1.shape[0])
		
	return err

	
arr = []
for tmp_png in [f for f in os.listdir('tmp') if not f.startswith('.')]:
	min_a = 9999999999
	min_png = None
	pic = cv2.imread('tmp/' + tmp_png)
		
	for directory in [f for f in os.listdir('templates') if not f.startswith('.')]:
		for png in [f for f in os.listdir('templates/' + directory) if not f.startswith('.')]:
			ref = cv2.imread('templates/' + directory + '/' + png)
			if mse(ref, pic) < min_a:
				min_a = mse(ref, pic)
				min_png = directory

	arr.append(min_png)
	
print ''.join(arr)
		