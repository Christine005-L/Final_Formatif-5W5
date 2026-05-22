import { transition, trigger, useAnimation } from '@angular/animations';
import { Component } from '@angular/core';
import { bounce, shake, shakeX, tada } from 'ng-animate';
import { lastValueFrom, timer } from 'rxjs';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    animations: [
        trigger('shakeX', [transition('* => *', useAnimation(shake, { params: { timing: 2 } }))]),
        trigger('bounce', [transition('* => *', useAnimation(bounce, { params: { timing: 4 } }))]),
        trigger('tada', [transition('* => *', useAnimation(tada, { params: { timing: 3 } }))])
    ],
    styleUrls: ['./app.component.css'],
    standalone: true
})
export class AppComponent {
  title = 'ngAnimations';
  shake: number = 0;
  bounce: number = 0;
  tada: number = 0;
  boucle: boolean = false;
  rotate: boolean = false;

  constructor() {
  }

  async rotateLeft() {
    this.rotate = true;
    await this.waitFor(1);
    this.rotate = false;
    await this.waitFor(0.001);
    this.rotate = true;
    await this.waitFor(0.999);
    this.rotate = false;
  }

  async animer() {
    console.log(this.boucle);
    while(this.boucle){
      this.playShake();
      await this.waitFor(2);
      this.playBounce();
      await this.waitFor(3);
      this.playTada();
      await this.waitFor(3);
    }
    this.playShake();
    await this.waitFor(2);
    this.playBounce();
    await this.waitFor(3);
    this.playTada();
    await this.waitFor(3);
  }

  playShake() {
    this.shake++;
  }

  playBounce() {
    this.bounce++;
  }

  playTada() {
    this.tada++;
  }

  async waitFor(delayInSeconds: number) {
    await lastValueFrom(timer(delayInSeconds * 1000));
  }
}
