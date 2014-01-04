Reaktion
========

*Reaktion* is an audio reactive animation toolkit for Unity.

[Demo](https://vine.co/v/h2H5Iqi3e3V)

It provides several components for reacting to audio signals.

- *Reaktor* - processes audio signals and provides information to other components.
- *ReaktorToTransform* - controls Transform with information from a Reaktor.
- *ReaktorToParticleSystem* - controls ParticleSystem with information from a Reaktor.
- *ReaktorToAnimator* - controls Animator with information from a Reaktor.
- *ReaktorToMessage* - sends messages with information from a Reaktor.

![Chart](http://keijiro.github.io/Reaktion/abstract.png)

Reaktor Component
-----------------

![Reaktor](http://keijiro.github.io/Reaktion/inspector-reaktor.png)

The *Reaktor* component is a kind of audio level meter that processes audio
signals and provides the level information to other components. It uses an
adaptive level detection algorithm to process wide range of input signals,
and it allows apps to receive several types of signals without calibrations.

ReaktorToTransform Component
----------------------------

![ReaktorToTransform](http://keijiro.github.io/Reaktion/inspector-transform.png)

ReaktorToParticleSystem Component
---------------------------------

![ReaktorToParticleSystem](http://keijiro.github.io/Reaktion/inspector-particle.png)

ReaktorToAnimator Component
---------------------------

![ReaktorToAnimator](http://keijiro.github.io/Reaktion/inspector-animator.png)

ReaktorToMessage Component
---------------------------

![ReaktorToMessage](http://keijiro.github.io/Reaktion/inspector-message.png)

License
-------

Copyright (C) 2013, 2014 Keijiro Takahashi

Permission is hereby granted, free of charge, to any person obtaining a copy of
this software and associated documentation files (the "Software"), to deal in
the Software without restriction, including without limitation the rights to
use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
the Software, and to permit persons to whom the Software is furnished to do so,
subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
