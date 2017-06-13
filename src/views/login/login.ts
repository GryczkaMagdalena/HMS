import {Router} from 'aurelia-router';
import {inject} from 'aurelia-framework';
import {EventAggregator} from 'aurelia-event-aggregator';
import {LoginService} from '../../services/login-service';


@inject(EventAggregator, Router, LoginService)
export class LoginView {

  loggedIn: boolean;
  userInfo: UserInfo;
  workerType: string;

  constructor(private eventAggregator: EventAggregator, private router: Router, private loginService: LoginService) {
    this.loggedIn = false;
  }

  logIn() {
    this.loginService.logIn(this.userInfo)
      .then(res => {
        let tmpUser = JSON.parse(JSON.stringify(res));
        this.loggedIn = true;
        this.workerType = tmpUser.roles[0];
        this.eventAggregator.publish('login::loggedIn', {loggedIn: this.loggedIn, workerType: this.workerType});
        if (this.workerType === 'customer') {
          this.router.navigateToRoute('base');
        } else if (this.workerType === 'worker') {
          this.router.navigateToRoute('employeeMainPanel');
        }
      })}
}

interface UserInfo {
  username: string;
  password: string;
}
