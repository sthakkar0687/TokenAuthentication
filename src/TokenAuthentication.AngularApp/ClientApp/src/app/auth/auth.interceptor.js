"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.AuthInterceptor = void 0;
var operators_1 = require("rxjs/operators");
var AuthInterceptor = /** @class */ (function () {
    function AuthInterceptor(router) {
        this.router = router;
    }
    AuthInterceptor.prototype.intercept = function (req, next) {
        var _this = this;
        if (localStorage.getItem('token') != null) {
            var clonedReq = req.clone({
                headers: req.headers.set('Authorization', 'Bearer ' + localStorage.getItem('token'))
            });
            return next.handle(clonedReq).pipe(operators_1.tap(function (suc) { }, function (err) {
                if (err.status = 401) {
                    localStorage.removeItem('token');
                    localStorage.removeItem('email');
                    localStorage.removeItem('userId');
                    localStorage.removeItem('expiry');
                    _this.router.navigate['/user/login'];
                }
            }));
        }
        next.handle(req.clone());
    };
    return AuthInterceptor;
}());
exports.AuthInterceptor = AuthInterceptor;
//# sourceMappingURL=auth.interceptor.js.map