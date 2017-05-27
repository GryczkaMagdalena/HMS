import * as gulp from 'gulp';
import build from './build';
import ftpDeploy from './ftp-deploy';


export default gulp.series(
  build,
  ftpDeploy
);
