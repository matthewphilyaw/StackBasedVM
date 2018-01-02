; Comments and empty lines are stripped when assembled
; and do not effect relative or absolute jumps to lines
; All jumps are based on the line, no labels are used yet

iconst 100      ; initial value
gstore 0        ; store value in data

; Substract one from the initial value above until it reaches zero
; and store the result each time
gload 0         ; load onto stack after store
iconst 1        ; load 1 into subtract
isub            ; subtract
gstore 0        ; store in data

; print the result of the subtraction
gload 0         ; load back to stack
print           ; print value (consumes stack)

; load the value back up (print consumes stack)
gload 0         ; load back onto stack
iconst 0        ; load compare value (in this case zero)
ieq             ; is the loaded value equal to zero?

; if the equality is false jump back 9 lines to the start of this routine
; Notice the blank lines and comments don't effect the jump
brf -9          ; reapeat subtraction till ieq is true

iconst 200      ; load value to indicate success, arbitrary value just chose 200
print           ; print 200 out
halt            ; stop program
