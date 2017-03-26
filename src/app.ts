import {Router, RouterConfiguration} from 'aurelia-router';

export class App {
  router: Router;

  configureRouter(config: RouterConfiguration, router: Router) {
    config.title = 'HMS';
    config.map([
      {route: ['', 'login'], moduleId:'login-view/login-view', title: 'Login'},
      {route: 'client/base-view', moduleId:'client/base-view/base-view', title: 'Hello Guest!'},
      {route: 'client/hotel-rules', moduleId:'client/hotel-rules/hotel-rules', title: 'Hotel Rules', name: 'hotelRules'},
      {route: 'client/room-service', moduleId:'client/room-service/room-service', title: 'Room Service', name: 'roomService'}
    ]);
    this.router = router;
  }
}
