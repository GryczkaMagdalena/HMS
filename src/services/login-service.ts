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

          sessionStorage.setItem('worker_type', this.userObject.roles[0]);


          if (this.userObject.roles[0] === 'customer') {
            this.getRoomNumber()
              .then(data => {
                let tmpData = JSON.parse(JSON.stringify(data));
                sessionStorage.setItem('room_number', tmpData.roomNumber);
              });

            sessionStorage.setItem('user_email', this.userObject.email);
          }

          resolve(this.userObject);
        })
        .catch(err => err.json().then(res => {console.log('err -> ', res); reject(new Error(res))}))
        .then(() => {this.loadHandlerService.setFree()});

    });
  }

  getRoomNumber() {
    this.loadHandlerService.setBusy();

    return new Promise((resolve, reject) => {
      this.httpFetch
        .fetch('/api/Room/RoomNumber')
        .then(response => response.json())
        .then(response => {
          resolve(response);
        })
        .catch(err => err.json().then(res => {console.log('err -> ', res); reject(new Error(res))}))
        .then(() => {this.loadHandlerService.setFree()});
    })
  }
}
