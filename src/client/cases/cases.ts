import {RouterConfiguration, Router} from 'aurelia-router';

export class Cases {

  router: Router;

  configureRouter(config: RouterConfiguration, router: Router) {
    config.map([
      {
        route: ['room-service', ''],
        name: 'roomService',
        title: 'Room Service',
        moduleId: 'client/cases/room-service/room-service',
        nav: true
      },
      {
        route: 'technical-issue',
        name: 'technicalIssue',
        title: 'Technical Issue',
        moduleId: 'client/cases/technical-issue/technical-issue',
        nav: true
      }
    ]);
    this.router = router;
  }

  constructor() {

  }
}
