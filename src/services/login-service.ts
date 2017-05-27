import {inject} from 'aurelia-framework';
import {HttpClient as HttpFetch, json} from 'aurelia-fetch-client';
import {EventAggregator} from 'aurelia-event-aggregator';
import {LoadHandlerService} from "./load-handler-service";

@inject(HttpFetch, EventAggregator, LoadHandlerService)
export class LoginService {
  userObject: any;
  constructor(public httpFetch: HttpFetch, private eventAggregator: EventAggregator, private loadHandlerService: LoadHandlerService) {
  }

  logIn(userInfo) {
    this.loadHandlerService.setBusy();

    let userObj = {
      Login: userInfo.username,
      Password: userInfo.password
    };

    return new Promise((resolve, reject) => {
      this.httpFetch
        .fetch('/api/Auth/Token', {
          method: 'post',
          body: JSON.stringify(userObj)
        })
        .then((response) => response.json())
        .then(data => {
          let tmpResponse = JSON.parse(JSON.stringify(data));
          this.userObject = tmpResponse.user;

          sessionStorage.setItem('session_token', ('Bearer ' + tmpResponse.token));

          sessionStorage.setItem('worker_type', tmpResponse.user.workerType);

          if (tmpResponse.user.workerType === 'None') {
            sessionStorage.setItem('room_number', tmpResponse.user.room.number);
            sessionStorage.setItem('user_email', tmpResponse.user.email);
          }

          resolve(data);
        })
        .catch(err => reject(err))
        .then(() => this.loadHandlerService.setFree());
    });
  }
}
