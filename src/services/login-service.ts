import {inject} from 'aurelia-framework';
import {HttpClient as HttpFetch, json} from 'aurelia-fetch-client';
import {EventAggregator} from 'aurelia-event-aggregator';

@inject(HttpFetch, EventAggregator)
export class LoginService {

  constructor(public httpFetch: HttpFetch, private eventAggregator: EventAggregator) {
  }

  logIn(userInfo) {
    //hardcoded for now; remove it in the future
    let userObj = {
      Login: "guest1",
      Password: "Gue$t1"
    };

    // let userObj = {
    //   Login: userInfo.username,
    //   Password: userInfo.password
    // };

    console.log("TO WYSYŁAM: ", userObj);

    return new Promise((resolve, reject) => {
      this.httpFetch
        .fetch('/api/Auth/Token', {
          method: 'post',
          body: JSON.stringify(userObj)
        })
        .then((response) => response.json())
        .then(data => {
          console.log('data',data);
          let tmpResponse = JSON.parse(JSON.stringify(data));
          let token = 'Bearer ' + tmpResponse.token;

          sessionStorage.setItem('session_token', token);

          resolve(data);
        })
        .catch(err => reject(err))
    });
  }
}
