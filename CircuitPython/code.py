import board
import sys
import time
import supervisor
import digitalio

def non_blocking_read():
    s = ""
    while supervisor.runtime.serial_bytes_available:
        s += sys.stdin.read(1)
    return s


import usb_hid as hid


from adafruit_hid.keyboard import Keyboard
from adafruit_hid.keyboard_layout_us import KeyboardLayoutUS

switch = digitalio.DigitalInOut(board.SWITCH)
switch.direction = digitalio.Direction.INPUT
switch.pull = digitalio.Pull.UP

k = Keyboard(hid.devices)
kl = KeyboardLayoutUS(k)
#i=0
s = ""
while True:
    s += non_blocking_read()
    print(i, s)

    if not switch.value and len(s) > 0:
        #print(i,"flush")
        kl.write(s)
        s = ""
    #i+=1
    time.sleep(.1)