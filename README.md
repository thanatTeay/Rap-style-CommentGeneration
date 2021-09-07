# Rap-style Commentary Generation in an Audience Participation Fighting Game
<!-- TABLE OF CONTENTS -->
<summary>Table of Contents</summary>
 <details open="open"> 
  <ol>
    <li>
      <a href="#about-the-project">About The Project</a>
    </li>
    <li>
      <a href="#Software-tools">Software tools</a>
      <ul>
        <li><a href="#AI_gBEAI">AI_gBEAI</a></li>
      </ul>
     <ul>
        <li><a href="#P-TwitchCapture">P-TwitchCapture</a></li>
      </ul>
     <ul>
        <li><a href="#text-to-speech-apg">Text to Speech APG</a></li>
      </ul>
    </li>
   <li>
      <a href="#Overview">Overview</a>
    </li>
  </ol>
</details>


<!-- ABOUT THE PROJECT -->
## About The Project
This paper presents a rap-style commentary system for enhancing Audience Participation Games (APGs), which are games--on live streaming services--that allow audiences to participate with an aim to promote their experience. We focus on common limitations in traditional APGs: humans cannot provide entertaining contents, both gameplay and commentaries, on the 24/7 basis. Therefore, an APG is developed where the game is played by artificial intelligence algorithms (AIs) and commentaries are generated in a musical fashion by the rap-style commentary system.
Descriptions are given on a design to make an existing fighting game become an entertaining APG through the process of AI strength adjustment based on chat messages from audiences, and on how to apply such strength adjustment to generate game commentaries. 
Experimental results show that the proposed system promotes audience experience.
* [About paper](https://drive.google.com/file/d/12k-n1bgWC0EGf1tEZEtIW5mb-Pv1EHpB/view?usp=sharing)
* [Website about This paper](https://thanatteay.github.io/Rap-style-CommentGeneration/public/index.html)


<!-- Software tools -->
## Software tools
This section should list any major frameworks that you built your project using. Leave any add-ons/plugins for the acknowledgements section. Here are a few examples.
* Visual studio
* Eclipse

### AI_gBEAI
FightingICE is the game in use for streaming. To use it as an APG, two gBEAIs are used; they receive **F**s from the previous component. A Commentary Mechanism is embedded; it monitors the AIs and generate comment scripts based on actions of the AIs and situation in the game. Comments to be spoken are exported as files, called "scripts."

### P-TwitchCapture
A program that connects to the Twitch channel to constantly pull messages from audiences from the Twitch server. Messages are analyzed, commands embedded in messages are extracted and processed, and then Social Facilitation parameters (denoted as $F$s) will be calculated and sent to AIs in FightingICE. In addition, Twitch Message Capturer also bypasses messages to Rap Synthesizer to inform it whether there is a new message.

### Text to Speech APG
A program for generating speech in a rap style. It receives scripts from the previous component, and determine which one to be used considering two things. First is whether there are some new messages embedding commands from audiences. Second is $HP$s of the two AI; it will focus on the AI that is dominating the game (i.e., has a higher HP). Speeches are rapped based on a given rhythm given in a file called  "Rap Flow." Speeches from Rap Synthesizer was streamed with FightingICE as one game system (they are actually two seperated applications since Rap Synthesizer is designed to be capable of working with other games as well).

<!-- Overview -->
## Overview
![Overview](https://drive.google.com/uc?export=view&id=1Cm0WxP83pK0OcjPKk6X4J3n0bJgdg9sM)

