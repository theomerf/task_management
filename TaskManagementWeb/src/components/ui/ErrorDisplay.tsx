import type { ApiErrorArrayItem, ApiErrorResponse, FormError } from "../../types/apiError";

interface ErrorDisplayProps {
    error: FormError | ApiErrorArrayItem[] | null;
    className?: string;
}

export function ErrorDisplay({ error, className = "text-red-700 text-left mt-2" }: ErrorDisplayProps) {
    if (!error) return null;

    if (typeof error === "string") {
        return (
            <div className="border-l-4 bg-red-50 py-2 border-red-400 pl-3">
                <p className={`${className} text-sm`}>
                    <span className="font-medium text-red-800">
                        {error}
                    </span>
                </p>
            </div>
        )
    }

    if (Array.isArray(error)) {
        return (
            <div className="mb-4 p-4 bg-red-50 border border-red-200">
                <div className="space-y-2">
                    {error.map((err, index) => (
                        err.code != "DuplicateUserName" && (
                            <div key={index} className="border-l-4 border-red-400 pl-3">
                                <p className={`${className} text-sm`}>
                                    <span className="font-medium text-red-800">
                                        {getErrorCodeDisplayName(err.code)}:
                                    </span>
                                    <span className="ml-1">{err.description}</span>
                                </p>
                            </div>
                        )
                    ))}
                </div>
            </div>
        );
    }

    if (typeof error === "object" && error !== null) {
        const apiError = error as ApiErrorResponse;

        return (
            <div className="mb-4 p-4 bg-red-50 border border-red-200 rounded-lg">
                {apiError.message && (!apiError.errors || Object.keys(apiError.errors).length === 0) && (
                    <div className={`${className} mb-2 font-semibold`}>
                        {apiError.message}
                    </div>
                )}

                {apiError.errors && Object.keys(apiError.errors).length > 0 && (
                    <div className="space-y-2">
                        {Object.entries(apiError.errors).map(([field, messages]) => (
                            <div key={field} className="border-l-4 border-red-400 pl-3">
                                {messages.map((message, index) => (
                                    <p key={`${field}-${index}`} className={`${className} text-sm`}>
                                        <span className="font-medium text-red-800">
                                            {getFieldDisplayName(field)}:
                                        </span>
                                        <span className="ml-1">{message}</span>
                                    </p>
                                ))}
                            </div>
                        ))}
                    </div>
                )}
            </div>
        );
    }

    return <p className={className}>Bilinmeyen bir hata oluştu.</p>;
}

function getErrorCodeDisplayName(code: string): string {
    const codeNames: Record<string, string> = {
        'DuplicateUserName': 'Kullanıcı Adı',
        'DuplicateEmail': 'E-posta',
        'InvalidPassword': 'Şifre',
        'InvalidUserName': 'Kullanıcı Adı',
        'InvalidEmail': 'E-posta',
        'PasswordRequiresLower': 'Şifre',
        'PasswordRequiresUpper': 'Şifre',
        'PasswordRequiresDigit': 'Şifre',
        'PasswordTooShort': 'Şifre',
    };

    return codeNames[code] || 'Hata';
}

function getFieldDisplayName(field: string): string {
    const fieldNames: Record<string, string> = {
        'UserName': 'Kullanıcı Adı',
        'Email': 'E-posta',
        'Password': 'Şifre',
        'FirstName': 'Ad',
        'LastName': 'Soyad',
        'PhoneNumber': 'Telefon',
        'BirthDate': 'Doğum Tarihi',
        'General': 'Genel'
    };

    return fieldNames[field] || field;
}