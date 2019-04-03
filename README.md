# Evolve Asteroids - Genetic Algorithm for Asteroids Game

[![Codacy Badge](https://api.codacy.com/project/badge/Grade/1dbdbbd63639406d9a82ef0feb640d16)](https://www.codacy.com/app/kleberandrade/evolve-asteroids?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=kleberandrade/evolve-asteroids&amp;utm_campaign=Badge_Grade)

An asteroids like game powered by artificial intelligence enhanced with evolutionary algorithms.

## Asteroids

Asteroids is a space-themed multidirectional shooter arcade game designed by Lyle Rains, Ed Logg, and Dominic Walsh and released in November 1979 by Atari, Inc. The player controls a single spaceship in an asteroid field which is periodically traversed by flying saucers. The object of the game is to shoot and destroy the asteroids and saucers, while not colliding with either, or being hit by the saucers' counter-fire. The game becomes harder as the number of asteroids increases. 

<p align="center">
  <img src="https://github.com/kleberandrade/evolve-asteroids/blob/master/Screenshots/original.png" height="400"/>
</p>

## Clone Game

<p align="center">
  <img src="https://github.com/kleberandrade/evolve-asteroids/blob/master/Screenshots/game.PNG" height="500"/>
</p>

## Evolutionary Algorithm

### Implementation

1.  Generate the initial population of individuals randomly. (First generation)
2.  Evaluate the fitness of each individual in that population (time limit, sufficient fitness achieved, etc.)
3.  Repeat the following regenerational steps until termination:
4.  Select the best-fit individuals for reproduction. (Parents)
5.  Breed new individuals through crossover and mutation operations to give birth to offspring.
6.  Evaluate the individual fitness of new individuals.
7.  Replace least-fit population with new individuals.

### Chromosome

A chromosome generally means a part of a gene. In the context of evolutionary algorithms, however, a chromosome represents a potential solution to the problem. Here’s an example:

<p align="center">
  <img src="https://github.com/kleberandrade/evolve-asteroids/blob/master/Screenshots/chromosome.png" height="500"/>
</p>

## Licença

    Copyright 2018 Kleber de Oliveira Andrade
    
    Permission is hereby granted, free of charge, to any person obtaining a copy
    of this software and associated documentation files (the "Software"), to deal
    in the Software without restriction, including without limitation the rights
    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    copies of the Software, and to permit persons to whom the Software is
    furnished to do so, subject to the following conditions:
    
    The above copyright notice and this permission notice shall be included in all
    copies or substantial portions of the Software.
    
    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
    SOFTWARE.