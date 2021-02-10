# XKBKeePassPlugin

*External Keyboard KeePass Plugin*

KeePass Plugin to connect an external keyboard made with an Adafruit ItsyBitsy nRF52 Express. (A switch is required, standard ItsyBitsys dont have one.)


1. Get an Adafruit ItsyBitsy nRF52 Express. 
	(Basically you need an CircuitPython platform with USB connection and a switch on it)

2. Install the plugin by copying XKBKeePassPlugin.dll to KeePass/Plugins

3. Copy the CircuitPython source to your ItsyBitsy.

4. Plugin your ItsyBitsy and start KeePass.

5. Open the XKBOptions and choose your ComPort Device.

6. You are ready to go:<P>
Use the context menu in kee pass on an entry to send password to the defined com port.
Set the cursor to the login field.
Press the SW Switch on ItsyBitsy: it will type in the password as an external keyboard. 
	
	
CircuitPython Code:

``
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
``
