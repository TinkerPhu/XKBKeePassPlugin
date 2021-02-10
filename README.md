# XKBKeePassPlugin
External Keyboard KeePass Plugin
KeePass Plugin to connect an external keyboard made with an Adafruit ItsyBitsy nRF52 Express. (A switch is required, standard ItsyBitsys dont have one.)


1. Get an Adafruit ItsyBitsy nRF52 Express. 
	(Basically you need an CircuitPython platform with USB connection and a switch on it)

2. Install the plugin by copying XKBKeePassPlugin.dll to KeePass/Plugins

3. Copy the CircuitPython source to your ItsyBitsy.

4. Plugin your ItsyBitsy and start KeePass.

5. Open the XKBOptions and choose your ComPort Device.

6. You are ready to go.
Use the context menu in kee pass on an entry to send password to the defined com port.
Set the cursor to the login field.
Press the SW Switch on ItsyBitsy: it will type in the password as an external keyboard. 