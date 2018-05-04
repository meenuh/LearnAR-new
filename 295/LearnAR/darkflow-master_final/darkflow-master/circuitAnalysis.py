import os


def execute_model():
    path = 'D:/Unity/LearnAR/295/LearnAR/darkflow-master_final/darkflow-master/'
    cmd = 'python ' + path + 'flow --imgdir ' + path + 'sample_img' + ' --model ' + path + 'cfg/tiny-yolo-voc-4c.cfg' + ' --load 107500 --threshold 0.2 --json'

    os.system(cmd)
    return


def execute_get_labels():
    path = 'D:/Unity/LearnAR/295/LearnAR/darkflow-master_final/darkflow-master/'
    cmd = 'python ' + path + 'test.py'

    os.system(cmd)
    return

