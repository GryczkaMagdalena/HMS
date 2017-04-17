import {Router, RouterConfiguration} from 'aurelia-router';

export class App {
  router: Router;

  configureRouter(config: RouterConfiguration, router: Router) {
    config.title = 'HMS';
    config.map([
      {route: ['', 'login'], moduleId:'views/login/login', title: 'Login'},
      {route: 'client/base-view', moduleId:'views/client/base/base', title: 'Hello Guest!'},
      {route: 'client/hotel-rules', moduleId:'views/client/hotel-rules/hotel-rules', title: 'Hotel Rules', name: 'hotelRules'},
      {route: 'client/cases', moduleId:'views/client/cases/cases', title: 'Cases', name: 'cases'}
    ]);
    this.router = router;
  }
}
