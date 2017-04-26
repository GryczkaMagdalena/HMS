import {inject} from 'aurelia-framework';
import {HttpClient as HttpFetch, json} from 'aurelia-fetch-client';
import {EventAggregator} from 'aurelia-event-aggregator';
import {AureliaCookie} from 'aurelia-cookie';
import * as $ from "jquery";

@inject(HttpFetch, EventAggregator)
export class LoginService {

  constructor(public httpFetch: HttpFetch, private eventAggregator: EventAggregator) {
  }

  logIn(userInfo) {
    let userObj = {
      Login: "draggie",
      Password: "R@yman12"
    };

    // let userObj = {
    //   Login: userInfo.username,
    //   Password: userInfo.password
    // };

    console.log("TO WYSYŁAM: ", userObj);

    var promise = new Promise((resolve, reject) => {
      $.ajax("http://hotelmanagementsystem.azurewebsites.net/api/auth/login", {
        data: JSON.stringify(userObj),
        contentType: 'application/json',
        type: 'POST',
        success: (data, textStatus, request) => {
          console.log(request.getAllResponseHeaders());
        },
        error: (request, textStatus, errorThrown) => {

        }
      })
    });



    // var promise = new Promise((resolve, reject) => {
    //   this.httpFetch
    //     .fetch('/api/auth/login', {
    //       method: 'post',
    //       body: JSON.stringify(userObj)
    //     })
    //     .then((response) => {
    //       console.log(response);
    //       // console.log(response.headers.get('Set-Cookie'));
    //       return response.json();
    //     })
    //     .then(data => {
    //       console.log('data',data);
    //       // console.log(getCookie('io'));
    //       // let tmpResponse = JSON.parse(JSON.stringify(data));
    //       //
    //       // let token = tmpResponse.token.tokenType + ' ' + tmpResponse.token.token;
    //       // let backendUrl = tmpResponse.endpointAddress;
    //       //
    //       // sessionStorage.setItem('session_token', token);
    //       // sessionStorage.setItem('backend_url', backendUrl);
    //       //
    //       // this.httpFetch.configure(config => {
    //       //   config.withBaseUrl(backendUrl);
    //       // });
    //
    //       resolve(data);
    //     })
    //     .catch(err => reject(err))
    // });
    // return promise;
  }
}
