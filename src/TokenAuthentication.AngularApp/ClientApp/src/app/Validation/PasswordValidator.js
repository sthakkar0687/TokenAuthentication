"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.PasswordValidator = void 0;
function PasswordValidator(control) {
    var password = control.get('password');
    var confirmPassword = control.get('confirmPassword');
    if (password.pristine || confirmPassword.pristine) {
        return null;
    }
    return (password && confirmPassword && password.value === confirmPassword.value) ? null : { mismatch: true };
}
exports.PasswordValidator = PasswordValidator;
//# sourceMappingURL=PasswordValidator.js.map