#YS Game1
personal project for portfolio<br/>
started from 9 Aug 2021
Demo Video : https://youtu.be/3lAhOwVzNTM
contact : dydtn8575@naver.com
gitHub : https://github.com/LeeYongSu1

##Intoduction
This is a simple RPG game. I mostly focues on the components below.
#####If you want to download the whole project, please visit here. https://drive.google.com/drive/u/1/folders/1fw38acjuyixO2GnBiVd758XYthjCqTJZ
#####In this git repository I only uploaded part of the project. If you want to see the codes, enter "/Assets/01.Scripts/..".


1. It can move as followed with keboard and mouse input.
2. After attacking, the character can smoothly move to the point where the mouse clicked.
3. There are two attack motions. The basic one is when you click once. The other one is when you click agagin after basic attack in short time.
4. When attack skill is exceuted, the attack object goes to the point mouse with physical energy.
5. The enemy position is controlled by ***Navigation Agent***.
6. The enemy is creataed by ***Object Pooling***.
7. UI designs. ( ex. showing time limit after using attack skils, starting and ending views)

##Develpoment Environment
- Unity 2018
- Visual Studio

##Screenshots
<img src="/images/screenshot1.png></img>
<img src="/images/screenshot2.png></img>
<img src="/images/screenshot3.png></img>

##How to run
There is /build/Lee.exe in project folder. Double click it.

##How to play
- Attack
Click the enemy with mouse.
- Move
Use keyboard 'W(↑)' 'A(←)' 'S(↓)' 'D(→)' to the place you want to move. </br>
Use mouse click. The character will turn to it.
- Skill
Press keyboard '1~3'. </br>
(The 3rd skill is going to be updated.)
</br>
</br>
When you kill all of the enemies, you can see the Victory mark.
When you are killed by the enemies, you can see the Defeat mark.