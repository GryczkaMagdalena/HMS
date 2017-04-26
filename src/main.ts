import {Aurelia} from 'aurelia-framework'
import environment from './environment';
import {HttpClient as HttpFetch} from 'aurelia-fetch-client';

//Configure Bluebird Promises.
(<any>Promise).config({
  warnings: {
    wForgottenReturn: false
  }
});

export function configure(aurelia: Aurelia) {
  let http = new HttpFetch();

  aurelia.use
    .standardConfiguration()
    .plugin('aurelia-cookie')
    .feature('resources');

  if (environment.debug) {
    aurelia.use.developmentLogging();
  }

  if (environment.testing) {
    aurelia.use.plugin('aurelia-testing');
  }

  let container = aurelia.container;

  http.configure(config => {
    config
      .useStandardConfiguration()
      .withBaseUrl('http://hotelmanagementsystem.azurewebsites.net')
      .withDefaults({
        credentials: 'same-origin',
        headers: {
          'Content-Type': 'application/json',
          'Accept': 'application/json',
        }
      });
    // .withInterceptor({
    //   request(request) {
    //     request.headers.append('Authorization', sessionStorage.getItem('session_token'));
    //     return request;
    //   },
    //   response(response) {
    //     return response;
    //   }
    // });
  });

  container.registerInstance(HttpFetch, http);

  aurelia.start().then(() => aurelia.setRoot());
}
