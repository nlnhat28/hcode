const permission = {
    problem: {
        create: 101,
        update: 102,
        delete: 103,
        submit: 104,
    },
    contest: {
        create: 201,
        update: 202,
        delete: 203,
        submit: 204,
    },
    admin: []  // Rỗng là có full quyền
}
export default permission;