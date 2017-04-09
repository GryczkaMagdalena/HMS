import {RouterConfiguration, Router} from 'aurelia-router';

export class Issues {

  router: Router;

  configureRouter(config: RouterConfiguration, router: Router) {
    config.map([
      {
        route: ['room-service'],
        name: 'roomService',
        title: 'Room Service',
        moduleId: 'client/issues/room-service/room-service',
        nav: true
      },
      {
        route: 'technical-issue',
        name: 'technicalIssue',
        title: 'Technical Issue',
        moduleId: 'client/issues/technical-issue/technical-issue',
        nav: true
      }
    ]);
    this.router = router;
  }
}
