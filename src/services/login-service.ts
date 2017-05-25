import {inject} from 'aurelia-framework';
import {HttpClient as HttpFetch, json} from 'aurelia-fetch-client';
import {EventAggregator} from 'aurelia-event-aggregator';

@inject(HttpFetch, EventAggregator)
export class LoginService {
  userObject: any;
  constructor(public httpFetch: HttpFetch, private eventAggregator: EventAggregator) {
  }

  logIn(userInfo) {

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
            sessionStorage.setItem('room_id', tmpResponse.user.room.roomID);
          }

          resolve(data);
        })
        .catch(err => reject(err))
    });
  }
}
