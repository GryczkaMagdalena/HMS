import * as gulp from "gulp";
let gutil = require('gulp-util');
let ftp = require('vinyl-ftp');

let user = 'hotelmanagementsystem-ui\\hmsftp';
let password = 'KhazadDum';
let host = 'waws-prod-sn1-085.ftp.azurewebsites.windows.net';
let remotePath = '/site/wwwroot';
let localFiles = ['index.html', 'scripts/*', 'src/assets/*'];

function getFtpConnection() {
  return ftp.create({
    host: host,
    user: user,
    password: password
  });
}

export default function ftpDeploy() {
  let conn = getFtpConnection();

  return gulp.src(localFiles, {base: '.', buffer: false})
    .pipe(conn.newer(remotePath))
    .pipe(conn.dest(remotePath));
}
