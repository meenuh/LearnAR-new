import cv2
import numpy as np
from darkflow.net.build import TFNet
import matplotlib.pyplot as plt
import json

#%config InlineBackend.figure_format = 'svg'

options = {
  'model': 'cfg/tiny-yolo-voc-4c.cfg',
  'load': 111375,
  'threshold': 0.1
}

tfnet = TFNet(options)

img = cv2.imread('sample_img/circuit.jpg')
result = tfnet.return_predict(img)

print(result)

for i in range (0,len(result)):
    tl = (result[i]['topleft']['x'], result[i]['topleft']['y'])
    br = (result[i]['bottomright']['x'], result[i]['bottomright']['y'])
    label = result[i]['label']

    img = cv2.rectangle(img, tl, br, (0, 255, 0), 7)
    img = cv2.putText(img, label, tl, cv2.FONT_HERSHEY_DUPLEX, 1, (0, 0, 0), 2)

cv2.imwrite('label_img/circuit_label.jpg', img)
for i in range(len(result)):
    # for key, value in result[i].items():
    del result[i]['confidence']
        # if type(value) is np.float32:
        #     value = value.item()
        #     print(type(value))
print (result)
res = json.dumps(result)

with open('sample_img/out/circuit.json', 'w') as fh:
    fh.write(res)

# plt.imshow(img)
# plt.show()