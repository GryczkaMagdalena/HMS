import {Router} from 'aurelia-router';
import {inject} from 'aurelia-framework';
import {EventAggregator} from 'aurelia-event-aggregator';
import {LoginService} from '../../services/login-service';


@inject(EventAggregator, Router, LoginService)
export class LoginView {

  loggedIn: boolean;
  userInfo: UserInfo;
  workerType: string;

  constructor(public eventAggregator: EventAggregator, public router: Router, public loginService: LoginService) {
    this.loggedIn = false;
  }

  logIn() {
    this.loginService.logIn(this.userInfo)
      .then(res => {
        let tmpRes = JSON.parse(JSON.stringify(res));
        this.loggedIn = true;
        this.workerType = tmpRes.user.workerType;
        this.eventAggregator.publish('login::loggedIn', {loggedIn: this.loggedIn, workerType: this.workerType});
        if (this.workerType === 'None') {
          this.router.navigateToRoute('base');
        } else if (this.workerType === 'Cleaner' || this.workerType === 'Technician') {
          this.router.navigateToRoute('employeeMainPanel');
        }
      })
      .catch(err => err.json().then(res => console.log(res)));
  }
}

interface UserInfo {
  username: string;
  password: string;
}
