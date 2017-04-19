import {Router} from 'aurelia-router';
import {inject} from 'aurelia-framework';
import {EventAggregator} from 'aurelia-event-aggregator';
import {LoginService} from '../../services/login-service';


@inject(EventAggregator, Router, LoginService)
export class LoginView {

  loggedIn: boolean;
  userInfo: UserInfo;

  constructor(public eventAggregator: EventAggregator, public router: Router, public loginService: LoginService) {
    this.loggedIn = false;
  }

  logIn() {
    console.log("dupadupa", this.userInfo);

    this.loginService.logIn(this.userInfo)
      .then(res => {
        this.loggedIn = true;
        this.eventAggregator.publish('login::loggedIn', {loggedIn: this.loggedIn});
        this.router.navigateToRoute('base');
      })
      .catch(err => console.log(err));
  }
}

interface UserInfo {
  username: string;
  password: string;
}
