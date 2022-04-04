# Home

<link rel="stylesheet" href="https://use.fontawesome.com/releases/v5.12.1/css/all.css" crossorigin="anonymous">

<a class="btn" href="https://arxiv.org/abs/2202.12884">
	Paper <i class="fa fa-paperclip"></i>
</a> <a class="btn" href="https://github.com/BenedictWilkins/world-of-bugs">
    Github <i class="fab fa-github ml-2 "></i>
</a>

World of Bugs (WOB) is a platform that supports research in Automated Bug Detection (ABD) in video games.

## What is Automated Bug Detection (ABD)?

Anyone who has played a game has encountered a bug at some point and probably knows that they come in all shapes and sizes. During development, the development and Quality Assurance (QA) teams take costly steps to try and prevent bugs from making it to release. Many different strategies are used, ranging from [Unit Testing](https://en.wikipedia.org/wiki/Unit_testing) to [Functional Testing](https://en.wikipedia.org/wiki/Game_testing), some of which can be (at least partially) automated. 

Modern ABD attempts to automated aspects of functional testing, which typically includes playing the game. However it may also appears when performing [Regression Testing](https://en.wikipedia.org/wiki/Regression_testing), or in any other strategy that would otherwise involve significant investment of time dedicated to actually playing the game.

## Automated Game Playing

A large part of ABD involves devising systems that can automatically play and explore video game just like a human game tester would. An automated game player is referred to as an _agent_. These game playing agents takes actions in the game environment in the same way that a human would using a virtual "gamepad" and observe what happens, and taking subsequent actions in order to achieve a predefined goal.

Perhaps unsurprisingly developing game playing agents can be very difficult. Games are often made to present some challenge to the player with a variety of obstacles, puzzles and enemies. Only in the last few years have agents been able to play with any video games with any real skill. Early attempts at playing [Atari 2600 games with deep reinforcement learning](https://arxiv.org/abs/1312.5602) started a trend in researchers attempting to tackle ever more complex games. More recently progress has been made on games that are widely recognised as requiring significant skill such as [Dota2](https://arxiv.org/abs/1912.06680) and [Starcraft](https://www.nature.com/articles/s41586-019-1724-z). These systems still require an unrealistic amount of resources to be practical for testing video games, nevertheless they are perhaps an early indication that ABD may form part of the standard toolkit for video game testing in the not so far future. 

<img src="imgs/Atari.gif"  width=50% style="display:block; margin-left:auto; margin-right:auto; margin-bottom:1rem;"> 


In the majority of research into automated game playing, an agent has a goal, usually to win. In the Atari 2600 example, the goal is specified using a reward that is derived from the in-game score. For ABD however, what is the goal? and more importantly, how should it be specified? At a high level, the goal is to explore the game to find bugs. The agent should have good _coverage_, meaning that it should explore as much of the game as possible, visit all the areas, complete all the levels, fight all the enemies etc. We might think of the agent as the most eager completionist. Not only does it want to find all the collectables, but also see everything there is to see. In this sense, the agent is a game player of the most hardcore variety. 

The question of how to specify the goal of such an agent is an open one. Placing this question aside for a moment in favour of a more immediate problem, how is an agent to know that is has encountered a bug on its travels through its favourite game? This is the problem of bug identification. 

## Automated Bug Identification

The second facet to the problem of ABD is that of bug identification. When a human tester encounters a bug, how to they know? It could just be some wacky game mechanic, perhaps horses are supposed to do handstands.

<img src="imgs/the-witcher-3-roach.png"  width=80% style="display:block; margin-left:auto; margin-right:auto; margin-bottom:1rem;"> 

The waters become a murky around this point. We might formally say that a human tester has a specification that they work from that explicitly states "horses don't do handstands", but this just isn't the case. 


TO BE CONTINUED...