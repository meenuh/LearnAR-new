import os


def execute_model():
    cmd = 'python flow --imgdir sample_img/ --model cfg/tiny-yolo-voc-4c.cfg --load 107500  --threshold 0.15 --json'
    os.system(cmd)
    return


def cleanup(img, json_file):

    os.remove(img)
    os.remove(json_file)
    return


def calc_points():

    return
