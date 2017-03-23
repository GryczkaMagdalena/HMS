import {Router, RouterConfiguration} from 'aurelia-router';

export class App {
  router: Router;

  configureRouter(config: RouterConfiguration, router: Router) {
    config.title = 'HMS';
    config.map([
      {route: ['', 'login'], moduleId:'login-view/login-view', title: 'Login'},
      {route: 'base-view', moduleId:'client/base-view/base-view', title: 'Hello Guest!'}
    ]);
    this.router = router;
  }
}
