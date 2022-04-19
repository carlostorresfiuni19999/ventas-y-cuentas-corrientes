// Next.js API route support: https://nextjs.org/docs/api-routes/introduction

export default function handler(req, res) {
    res.status(200).json([
        {id: 1, cliente: 'Carlos Torres', cin: '1234567', estado: 'Pendiente', montTotal:  12500000, saldo: 12500000, cantidad: 1},
        {id: 2, cliente: 'Carlos Torres', cin: '1234567', estado: 'Completo', montTotal:  11000000, saldo: 0, cantidad: 1},
        {id: 3, cliente: 'Carlos Torres', cin: '1234567', estado: 'Pendiente', montTotal:  18000000, saldo: 12000000, cantidad: 1},
        {id: 4, cliente: 'Carlos Torres', cin: '1234567', estado: 'Completo', montTotal:  11000000, saldo: 0, cantidad: 1}
    ])
  }
  