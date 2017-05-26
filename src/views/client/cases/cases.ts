import {RouterConfiguration, Router} from 'aurelia-router';

export class Cases {

  cases: {}[];
  router: Router;

  configureRouter(config: RouterConfiguration, router: Router) {
    config.map([
      {
        route: ['room-support', ''],
        name: 'roomSupport',
        title: 'Room Service',
        moduleId: 'views/client/cases/room-support/room-support',
        nav: true
      },
      {
        route: 'technical-issue',
        name: 'technicalIssue',
        title: 'Technical Issue',
        moduleId: 'views/client/cases/technical-issue/technical-issue',
        nav: true
      }
    ]);
    this.router = router;
  }

  constructor() { }


}
