import {Redirect, NavigationInstruction, RouterConfiguration, Router, Next} from 'aurelia-router';
import {inject} from 'aurelia-framework';
import {EventAggregator} from 'aurelia-event-aggregator';

@inject(EventAggregator)
export class App {

  loggedIn: boolean;
  router: Router;

  constructor(public eventAggregator: EventAggregator) {
  }

  configureRouter(config: RouterConfiguration, router: Router) {
    config.title = 'HMS';
    config.map([
      {
        route: ['', 'login'],
        moduleId: 'views/login/login',
        title: 'Login',
        name: 'login',
        settings: {
          publicOnly: true
        }
      },
      {
        route: 'client/base',
        moduleId: 'views/client/base/base',
        title: 'Hello Guest!',
        name: 'base',
        settings: {
          auth: true
        }
      },
      {
        route: 'client/hotel-rules',
        moduleId: 'views/client/hotel-rules/hotel-rules',
        title: 'Hotel Rules',
        name: 'hotelRules',
        settings: {
          auth: true
        }
      },
      {
        route: 'client/cases',
        moduleId: 'views/client/cases/cases',
        title: 'Cases',
        name: 'cases',
        settings: {
          auth: true
        }
      }
    ]);
    config.addPipelineStep('authorize', AuthorizeStep);

    this.router = router;
    this.loggedIn = false;
  }

  attached() {
    this.eventAggregator.subscribe('login::loggedIn', (data) => {
      AuthorizeStep.auth.isAuthenticated = data.loggedIn;
    });
  }

}


class AuthorizeStep {

  static auth = {
    isAuthenticated: !!sessionStorage.getItem('session_token')
  };

  run(navigationInstruction: NavigationInstruction, next: Next) {
    let isLoggedIn = AuthorizeStep.auth.isAuthenticated;

    // currently active route config
    let currentRoute = navigationInstruction.config;

    // settings object will be preserved during navigation
    let loginRequired = currentRoute.settings && currentRoute.settings.auth === true;

    if (isLoggedIn === false && loginRequired === true) {
      return next.cancel(new Redirect('login'));
    }

    let publicOnly = currentRoute.settings && currentRoute.settings.publicOnly === true;
    if (isLoggedIn === true && publicOnly === true) {
      return next.cancel(new Redirect('client/base'));
    }

    return next();
  };

}
