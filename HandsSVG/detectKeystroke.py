# KeyLogger.py
# show a character key when pressed without using Enter key
# hide the Tkinter GUI window, only console shows

import Tkinter as tk

def key(event):
    if event.keysym == 'Escape':
        root.destroy()
    print event.char, event.keysym

root = tk.Tk()
print "Press a key (Escape key to exit):"
root.bind_all('<Key>', key)
# don't show the tk window
root.withdraw()
root.mainloop()
